using ShiftTrack.Domain.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

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
    public VacationStatus Status { get; private set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public long? ApprovedEmployeeId { get; set; }
    public Employee ApprovedEmployee { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // State machine:
    //   None ──Submit──► PendingApproval
    //   PendingApproval ──Approve(DepartmentDirector)──► ApprovedByDepartmentDirector
    //   PendingApproval, ApprovedByDepartmentDirector ──Approve(UnitDirector)──► ApprovedByUnitDirector
    //   PendingApproval, ApprovedByDepartmentDirector ──Reject(any level)──► Rejected
    //   ApprovedByUnitDirector ──Reject(UnitDirector)──► Rejected
    // Any other change is not possible.

    private static readonly IReadOnlyDictionary<(VacationStatus From, VacationApprovalLevel Level), VacationStatus>
        ApproveTransitions = new Dictionary<(VacationStatus, VacationApprovalLevel), VacationStatus>
        {
            [(VacationStatus.PendingApproval, VacationApprovalLevel.DepartmentDirector)] =
                VacationStatus.ApprovedByDepartmentDirector,
            [(VacationStatus.PendingApproval, VacationApprovalLevel.UnitDirector)] =
                VacationStatus.ApprovedByUnitDirector,
            [(VacationStatus.ApprovedByDepartmentDirector, VacationApprovalLevel.UnitDirector)] =
                VacationStatus.ApprovedByUnitDirector
        };

    private static readonly IReadOnlyDictionary<VacationApprovalLevel, VacationStatus[]> RejectableStatuses =
        new Dictionary<VacationApprovalLevel, VacationStatus[]>
        {
            [VacationApprovalLevel.DepartmentDirector] =
                [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector],
            [VacationApprovalLevel.UnitDirector] =
                [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector,
                    VacationStatus.ApprovedByUnitDirector]
        };

    /// <summary>A draft can be submitted for approval.</summary>
    public bool CanSubmit => Status == VacationStatus.None;

    public bool CanApprove(VacationApprovalLevel level) => ApproveTransitions.ContainsKey((Status, level));

    public bool CanReject(VacationApprovalLevel level) => RejectableStatuses[level].Contains(Status);

    public void Submit()
    {
        if (!CanSubmit)
            throw new VacationStatusTransitionException(Status, "submitted");

        Status = VacationStatus.PendingApproval;
    }

    public void Approve(VacationApprovalLevel level)
    {
        if (!ApproveTransitions.TryGetValue((Status, level), out var next))
            throw new VacationStatusTransitionException(Status, $"approved by {level}");

        Status = next;
    }

    public void Reject(VacationApprovalLevel level)
    {
        if (!CanReject(level))
            throw new VacationStatusTransitionException(Status, $"rejected by {level}");

        Status = VacationStatus.Rejected;
    }
}