using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using ShiftTrack.Scheduling.Options;
using SchedulingOptions = ShiftTrack.Scheduling.Options.SchedulingOptions;

namespace ShiftTrack.Scheduling;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Quartz with a hosted scheduler and the given jobs.
    /// Each job is scheduled by its cron expression from "Scheduling:Jobs:{JobName}".
    /// </summary>
    /// <example>
    /// builder.Services.AddScheduling(builder.Configuration, jobs => jobs.AddJob&lt;VacationAccrualJob&gt;());
    /// </example>
    public static IServiceCollection AddScheduling(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<SchedulingBuilder> configure)
    {
        var section = configuration.GetSection(SchedulingOptions.SectionName);
        services.Configure<SchedulingOptions>(section);

        var options = section.Get<SchedulingOptions>() ?? new SchedulingOptions();

        var builder = new SchedulingBuilder();
        configure(builder);

        var timeZone = FindTimeZone(options.TimeZone);

        services.AddQuartz(quartz =>
        {
            foreach (var job in builder.Jobs)
            {
                var schedule = GetSchedule(options, job.Name);

                if (!schedule.Enabled)
                    continue;

                var jobKey = new JobKey(job.Name);

                job.AddJob(quartz, jobKey, jobConfigurator => jobConfigurator
                    .WithDescription(schedule.Description ?? job.Name)
                    .StoreDurably());

                quartz.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity($"{job.Name}.cron")
                    .WithCronSchedule(schedule.Cron, cron => cron
                        .InTimeZone(timeZone)
                        .WithMisfireHandlingInstructionFireAndProceed()));

                if (schedule.RunOnStartup)
                {
                    quartz.AddTrigger(trigger => trigger
                        .ForJob(jobKey)
                        .WithIdentity($"{job.Name}.startup")
                        .StartAt(DateTimeOffset.UtcNow.AddSeconds(Math.Max(schedule.StartupDelaySeconds, 0))));
                }
            }
        });

        services.AddQuartzHostedService(hostedService => hostedService.WaitForJobsToComplete = true);

        return services;
    }

    private static readonly Dictionary<string, string> TimeZoneAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Europe/Kyiv"] = "Europe/Kiev",
        ["Europe/Kiev"] = "Europe/Kyiv"
    };

    /// <summary>Older tz databases (e.g. ICU on some Windows versions) know Kyiv only as "Europe/Kiev".</summary>
    private static TimeZoneInfo FindTimeZone(string timeZoneId)
    {
        if (TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var timeZone))
            return timeZone;

        if (TimeZoneAliases.TryGetValue(timeZoneId, out var alias)
            && TimeZoneInfo.TryFindSystemTimeZoneById(alias, out timeZone))
            return timeZone;

        throw new InvalidOperationException($"Time zone \"{timeZoneId}\" from \"{SchedulingOptions.SectionName}:TimeZone\" was not found.");
    }

    private static JobScheduleOptions GetSchedule(SchedulingOptions options, string jobName)
    {
        if (!options.Jobs.TryGetValue(jobName, out var schedule))
        {
            throw new InvalidOperationException(
                $"Schedule for job \"{jobName}\" is not configured: add \"{SchedulingOptions.SectionName}:Jobs:{jobName}:Cron\".");
        }

        if (schedule.Enabled && !CronExpression.IsValidExpression(schedule.Cron))
        {
            throw new InvalidOperationException(
                $"Cron expression \"{schedule.Cron}\" of job \"{jobName}\" is not valid.");
        }

        return schedule;
    }
}
