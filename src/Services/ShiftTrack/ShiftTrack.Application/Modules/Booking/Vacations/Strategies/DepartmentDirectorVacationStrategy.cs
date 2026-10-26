using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class DepartmentDirectorVacationStrategy(
    ICurrentUserService currentUserService,
    IVacationRepository vacationRepository,
    IEmployeeShiftService employeeShiftService,
    IVacationShiftService vacationShiftService,
    IRoleScopeService roleScopeService) : IVacationStrategy
{
    private RoleScope Scope => roleScopeService.GetCurrentScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (vacation.AuthorId == currentUserService.Employee.Id
            && vacation.Status is VacationStatus.None)
        {
            await vacationRepository.UpdateVacationStatus(
                new UpdateVacationStatusDto(
                    vacation.Id,
                    VacationStatus.PendingApproval),
                cancellationToken);

            return;
        }

        ValidateVacationForStatusChange(vacation);

        if (vacation.EmployeeId == currentUserService.Employee.Id)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_OWN_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_OWN_VACATION));
        }

        EnsureCanApprove(vacation);

        await vacationRepository.UpdateVacationStatus(
            new UpdateVacationStatusDto(
                vacation.Id,
                VacationStatus.ApprovedByDepartmentDirector),
            cancellationToken);

        await vacationShiftService.SetVacationShifts(id, cancellationToken);
    }

    private void ValidateVacationForStatusChange(Vacation vacation)
    {
        if (!IsFromCurrentDepartment(vacation))
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_DEPARTMENT,
                nameof(VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_DEPARTMENT));
        }

        if (vacation.Status is VacationStatus.ApprovedByUnitDirector)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_UNIT_DIRECTOR_APPROVED_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_UNIT_DIRECTOR_APPROVED_VACATION));
        }
    }

    public async Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);
        
        ValidateVacationForStatusChange(vacation);
        EnsureCanReject(vacation);

        await vacationRepository.UpdateVacationStatus(
            new UpdateVacationStatusDto(
                vacation.Id,
                VacationStatus.Rejected),
            cancellationToken);

        await employeeShiftService.RestorePreviousEmployeeShifts(
            new RestoreEmployeeShiftsDto(
                [vacation.EmployeeId],
                vacation.StartDate,
                vacation.EndDate),
            cancellationToken);
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (!IsFromCurrentDepartment(vacation) && !IsOwn(vacation))
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_DEPARTMENT,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_DEPARTMENT));
        }

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(
        VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacations = await vacationRepository.GetFiltered(filter, cancellationToken);

        vacations = vacations
            .Where(x => IsOwn(x) || IsFromCurrentDepartment(x))
            .ToList();

        return vacations;
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        var scope = Scope;

        if (scope.IsEmpty)
            return null;

        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval],
            scope.UnitIds.ToList(),
            scope.DepartmentIds.ToList(),
            currentUserService.Employee.Id);
    }

    public VacationAccessDto GetAccess(Vacation vacation)
    {
        var currentEmployeeId = currentUserService.Employee.Id;
        var isOwnDraft = vacation.AuthorId == currentEmployeeId && vacation.Status == VacationStatus.None;
        var isFromDepartment = IsFromCurrentDepartment(vacation);

        var canApprove = isOwnDraft
                         || (isFromDepartment
                             && vacation.EmployeeId != currentEmployeeId
                             && vacation.Status is not (VacationStatus.ApprovedByDepartmentDirector
                                 or VacationStatus.ApprovedByUnitDirector));

        var canReject = isFromDepartment
                        && vacation.Status is not (VacationStatus.ApprovedByUnitDirector or VacationStatus.Rejected);

        return new VacationAccessDto(
            CanView: isFromDepartment || IsOwn(vacation),
            CanApprove: canApprove,
            CanReject: canReject,
            ApproveSubmitsForApproval: isOwnDraft);
    }

    private void EnsureCanApprove(Vacation vacation)
    {
        if (!GetAccess(vacation).CanApprove)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_VACATION));
        }
    }

    private void EnsureCanReject(Vacation vacation)
    {
        if (!GetAccess(vacation).CanReject)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_REJECT_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_REJECT_VACATION));
        }
    }

    /// <summary>The vacation is of an employee the director manages by the role bindings.</summary>
    private bool IsFromCurrentDepartment(Vacation vacation) => Scope.Covers(vacation.Employee);

    private bool IsOwn(Vacation vacation)
    {
        var currentEmployeeId = currentUserService.Employee.Id;

        return vacation.EmployeeId == currentEmployeeId || vacation.AuthorId == currentEmployeeId;
    }
}