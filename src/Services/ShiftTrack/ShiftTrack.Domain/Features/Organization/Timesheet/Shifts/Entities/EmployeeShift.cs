using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

public class EmployeeShift : AuditableEntity
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public long ShiftId { get; set; }
    public Shift Shift { get; set; }
}