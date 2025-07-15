using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Features.Booking.Vacations.Entities;

public class Vacation : AuditableEntity, ISoftDeletable
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Comment { get; set; }
    public int DaysCount { get; set; }
    public int DaysBalanceAtCreation { get; set; }
    public VacationType Type { get; set; }
    public VacationStatus Status { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public long? ApprovedEmployeeId { get; set; }
    public Employee ApprovedEmployee { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}