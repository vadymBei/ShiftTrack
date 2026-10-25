using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Quartz;
using Quartz.Impl.Matchers;

namespace ShiftTrack.Scheduling.Endpoints;

public static class SchedulingEndpoints
{
    /// <summary>
    /// Maps endpoints to view the scheduled jobs and to run a job manually:
    /// GET {prefix}/jobs, POST {prefix}/jobs/{name}/trigger
    /// and a dashboard page with the list of jobs at <paramref name="dashboardPath"/>.
    /// </summary>
    /// <remarks>The endpoints aren't protected, map them only for internal hosts.</remarks>
    public static RouteGroupBuilder MapSchedulingEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "api/scheduling", string? dashboardPath = "/")
    {
        if (dashboardPath is not null)
        {
            var html = SchedulingDashboard.Render("/" + prefix.Trim('/'));

            endpoints.MapGet(dashboardPath, () => Results.Content(html, "text/html; charset=utf-8"))
                .ExcludeFromDescription();
        }

        var group = endpoints.MapGroup(prefix).WithTags("Scheduling");

        group.MapGet("jobs", GetJobs)
            .WithName("GetScheduledJobs")
            .WithSummary("Scheduled jobs with their previous and next run");

        group.MapPost("jobs/{name}/trigger", TriggerJob)
            .WithName("TriggerScheduledJob")
            .WithSummary("Runs the job now, regardless of its schedule");

        return group;
    }

    private static async Task<IResult> GetJobs(ISchedulerFactory schedulerFactory, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var runningJobs = (await scheduler.GetCurrentlyExecutingJobs(cancellationToken))
            .Select(x => x.JobDetail.Key)
            .ToHashSet();

        var jobs = new List<ScheduledJobVm>();

        foreach (var jobKey in await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken))
        {
            var jobDetail = await scheduler.GetJobDetail(jobKey, cancellationToken);
            var triggers = await scheduler.GetTriggersOfJob(jobKey, cancellationToken);

            jobs.Add(new ScheduledJobVm(
                jobKey.Name,
                jobDetail?.Description,
                runningJobs.Contains(jobKey),
                triggers.Select(x => x.GetPreviousFireTimeUtc()).Where(x => x is not null).Max(),
                triggers.Select(x => x.GetNextFireTimeUtc()).Where(x => x is not null).Min()));
        }

        return Results.Ok(jobs.OrderBy(x => x.Name));
    }

    private static async Task<IResult> TriggerJob(string name, ISchedulerFactory schedulerFactory,
        CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        var jobKey = new JobKey(name);

        if (!await scheduler.CheckExists(jobKey, cancellationToken))
            return Results.NotFound($"Job '{name}' is not registered.");

        // [DisallowConcurrentExecution] on ScheduledJob queues the run if the job is already running.
        await scheduler.TriggerJob(jobKey, cancellationToken);

        return Results.Accepted();
    }
}
