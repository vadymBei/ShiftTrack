using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

public class Shift : AuditableEntity, ISoftDeletable
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public TimeSpan? WorkHours { get; set; }
    public ShiftType Type { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}