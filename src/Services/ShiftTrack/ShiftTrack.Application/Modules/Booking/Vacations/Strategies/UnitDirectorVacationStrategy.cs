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
    IVacationShiftService vacationShiftService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        ValidateVacationForStatusChange(vacation);

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
        var currentUnitId = currentUserService.Employee.Department?.UnitId;

        if (currentUnitId is null || filter.UnitId != currentUnitId)
        {
            return [];
        }

        return await vacationRepository.GetFiltered(filter, cancellationToken);
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        var currentUnitId = currentUserService.Employee.Department?.UnitId;

        if (currentUnitId is null)
            return null;

        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector],
            currentUnitId,
            null,
            null);
    }

    private bool IsFromCurrentUnit(Vacation vacation)
    {
        var currentUnitId = currentUserService.Employee.Department?.UnitId;

        return currentUnitId is not null
               && vacation.Employee.Department?.UnitId == currentUnitId;
    }
}