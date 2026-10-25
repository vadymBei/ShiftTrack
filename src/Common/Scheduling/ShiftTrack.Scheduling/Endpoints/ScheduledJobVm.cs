namespace ShiftTrack.Scheduling.Endpoints;

public record ScheduledJobVm(
    string Name,
    string? Description,
    bool IsRunning,
    DateTimeOffset? PreviousFireTime,
    DateTimeOffset? NextFireTime);
