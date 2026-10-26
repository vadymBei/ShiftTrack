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

public class UnitDirectorVacationStrategy(
    IVacationRepository vacationRepository,
    ICurrentUserService currentUserService,
    IEmployeeShiftService employeeShiftService,
    IVacationShiftService vacationShiftService,
    IRoleScopeService roleScopeService) : IVacationStrategy
{
    private RoleScope Scope => roleScopeService.GetCurrentScope(DefaultRolesCatalog.UNIT_DIRECTOR);

    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        ValidateVacationForStatusChange(vacation);
        EnsureCanApprove(vacation);

        await vacationRepository.UpdateVacationStatus(
            new UpdateVacationStatusDto(
                vacation.Id,
                VacationStatus.ApprovedByUnitDirector),
            cancellationToken);

        await vacationShiftService.SetVacationShifts(id, cancellationToken);
    }

    private void ValidateVacationForStatusChange(Vacation vacation)
    {
        if (!IsFromCurrentUnit(vacation))
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_UNIT,
                nameof(VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_UNIT));
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

        if (!IsFromCurrentUnit(vacation))
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_UNIT,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_UNIT));
        }

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(
        VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacations = await vacationRepository.GetFiltered(filter, cancellationToken);

        return vacations
            .Where(IsFromCurrentUnit)
            .ToList();
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        var scope = Scope;

        if (scope.IsEmpty)
            return null;

        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector],
            scope.UnitIds.ToList(),
            scope.DepartmentIds.ToList(),
            null);
    }

    public VacationAccessDto GetAccess(Vacation vacation)
    {
        var isFromUnit = IsFromCurrentUnit(vacation);

        return new VacationAccessDto(
            CanView: isFromUnit,
            CanApprove: isFromUnit && vacation.Status != VacationStatus.ApprovedByUnitDirector,
            CanReject: isFromUnit && vacation.Status != VacationStatus.Rejected,
            ApproveSubmitsForApproval: false);
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

    /// <summary>
    /// The vacation is of an employee the director manages by the role bindings, or the director's own
    /// (there is no role above the unit director, so they approve their own requests).
    /// </summary>
    private bool IsFromCurrentUnit(Vacation vacation)
    {
        var currentEmployeeId = currentUserService.Employee.Id;

        return vacation.EmployeeId == currentEmployeeId
               || vacation.AuthorId == currentEmployeeId
               || Scope.Covers(vacation.Employee);
    }
}