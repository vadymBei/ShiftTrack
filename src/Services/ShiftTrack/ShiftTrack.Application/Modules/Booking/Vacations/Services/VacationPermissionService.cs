using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

/// <summary>
/// Single source of truth for what the current user may do with a vacation.
/// Role-specific rules come from the vacation strategy, common rules live here.
/// </summary>
public class VacationPermissionService(
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IVacationStrategyFactory vacationStrategyFactory) : IVacationPermissionService
{
    public VacationPermissionsVm GetPermissions(Vacation vacation)
    {
        var access = vacationStrategyFactory.GetStrategy().GetAccess(vacation);

        return new VacationPermissionsVm
        {
            CanView = access.CanView,
            CanEdit = CanEdit(vacation),
            CanApprove = access.CanApprove,
            CanReject = access.CanReject,
            CanDelete = CanDelete(vacation),
            CanDownload = access.CanView && vacation.Status == VacationStatus.ApprovedByUnitDirector,
            ApproveSubmitsForApproval = access.CanApprove && access.ApproveSubmitsForApproval
        };
    }

    public bool CanEdit(Vacation vacation)
    {
        return IsAuthor(vacation) && vacation.Status == VacationStatus.None;
    }

    public bool CanDelete(Vacation vacation)
    {
        return employeeRoleChecker.HasCurrentUserSysAdminRole()
               || (IsAuthor(vacation)
                   && vacation.Status is VacationStatus.None or VacationStatus.PendingApproval);
    }

    private bool IsAuthor(Vacation vacation)
    {
        return vacation.AuthorId == currentUserService.Employee.Id;
    }
}
