using Quartz;

namespace ShiftTrack.Scheduling;

/// <summary>
/// Collects the jobs a service wants to run. Their schedules come from configuration.
/// </summary>
public class SchedulingBuilder
{
    private readonly List<JobRegistration> _jobs = new();

    internal IReadOnlyList<JobRegistration> Jobs => _jobs;

    /// <param name="name">Job name in "Scheduling:Jobs"; the class name by default.</param>
    public SchedulingBuilder AddJob<TJob>(string? name = null) where TJob : class, IJob
    {
        var jobName = string.IsNullOrWhiteSpace(name) ? typeof(TJob).Name : name;

        if (_jobs.Any(x => x.Name == jobName))
            throw new InvalidOperationException($"Job \"{jobName}\" is already registered.");

        _jobs.Add(new JobRegistration(
            jobName,
            (configurator, jobKey, configure) => configurator.AddJob<TJob>(jobKey, configure)));

        return this;
    }

    internal sealed record JobRegistration(
        string Name,
        Action<IServiceCollectionQuartzConfigurator, JobKey, Action<IJobConfigurator>> AddJob);
}
