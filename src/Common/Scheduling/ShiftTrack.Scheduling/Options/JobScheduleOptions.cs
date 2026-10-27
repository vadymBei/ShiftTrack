namespace ShiftTrack.Scheduling.Options;

public class JobScheduleOptions
{
    /// <summary>Quartz cron expression, e.g. "0 0 2 * * ?" — every day at 02:00.</summary>
    public string Cron { get; set; } = string.Empty;

    /// <summary>A disabled job is not scheduled at all.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Additionally runs the job once after the application starts.</summary>
    public bool RunOnStartup { get; set; }

    /// <summary>Delay of the startup run, so the application can finish starting.</summary>
    public int StartupDelaySeconds { get; set; } = 30;

    public string? Description { get; set; }
}
