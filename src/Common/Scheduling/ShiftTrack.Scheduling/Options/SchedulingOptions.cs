namespace ShiftTrack.Scheduling.Options;

/// <summary>
/// "Scheduling" section of the configuration.
/// </summary>
/// <example>
/// "Scheduling": {
///   "TimeZone": "Europe/Kyiv",
///   "Jobs": {
///     "VacationAccrualJob": { "Cron": "0 0 2 * * ?", "RunOnStartup": true }
///   }
/// }
/// </example>
public class SchedulingOptions
{
    public const string SectionName = "Scheduling";

    /// <summary>Time zone the cron expressions are evaluated in (IANA or Windows id).</summary>
    public string TimeZone { get; set; } = "Europe/Kyiv";

    /// <summary>Schedules keyed by job name (the job class name unless another name is given on registration).</summary>
    public Dictionary<string, JobScheduleOptions> Jobs { get; set; } = new();
}
