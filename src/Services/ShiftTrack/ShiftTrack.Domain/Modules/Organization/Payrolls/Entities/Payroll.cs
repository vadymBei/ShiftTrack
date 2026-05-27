using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;

public class Payroll : AuditableEntity
{
    public long Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkedHours { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalAmount { get; set; }
    public PayrollStatus Status { get; set; }
    public DateTime? PaidAt { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
}