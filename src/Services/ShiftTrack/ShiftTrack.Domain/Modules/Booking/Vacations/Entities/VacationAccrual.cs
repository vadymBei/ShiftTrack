using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

/// <summary>
/// Annual vacation days an employee is entitled to for one working year
/// (Law of Ukraine "On Vacations", art. 6, 10). Recalculated by the vacation accrual job.
/// </summary>
public class VacationAccrual : AuditableEntity
{
    public long Id { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    /// <summary>1 for the first working year since the employment start.</summary>
    public int WorkingYear { get; set; }

    public DateTime PeriodStart { get; set; }

    /// <summary>Actual end for a completed year, expected end for the current one.</summary>
    public DateTime PeriodEnd { get; set; }

    public decimal Days { get; set; }

    /// <summary>False while the first working year is still proportional (less than 6 months of service).</summary>
    public bool IsFullEntitlement { get; set; }
}
