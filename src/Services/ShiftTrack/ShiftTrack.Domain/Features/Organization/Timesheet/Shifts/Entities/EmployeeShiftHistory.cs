using ShiftTrack.Domain.Common.Abstractions;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

public class EmployeeShiftHistory : AuditableEntity
{
    public long Id { get; set; }
    public TimeSpan? PreviousStartTime { get; set; }
    public TimeSpan? PreviousEndTime { get; set; }
    public TimeSpan? NewStartTime { get; set; }
    public TimeSpan? NewEndTime { get; set; }
    
    public long EmployeeShiftId { get; set; }
    public EmployeeShift EmployeeShift { get; set; }

    public long? PreviousShiftId { get; set; }
    public Shift PreviousShift { get; set; }
    
    public long? NewShiftId { get; set; }
    public Shift NewShift { get; set; }
}