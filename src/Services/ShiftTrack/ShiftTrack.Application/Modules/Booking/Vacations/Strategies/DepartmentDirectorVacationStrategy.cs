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
    IVacationShiftService vacationShiftService) : IVacationStrategy
{
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

        if (!IsFromCurrentDepartment(vacation))
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
            .Where(x => x.AuthorId == currentUserService.Employee.Id
                        || IsFromCurrentDepartment(x))
            .ToList();

        return vacations;
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        var currentEmployee = currentUserService.Employee;

        if (currentEmployee.DepartmentId is null)
            return null;

        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval],
            null,
            currentEmployee.DepartmentId,
            currentEmployee.Id);
    }

    private bool IsFromCurrentDepartment(Vacation vacation)
    {
        var currentDepartmentId = currentUserService.Employee.DepartmentId;

        return currentDepartmentId is not null
               && vacation.Employee.DepartmentId == currentDepartmentId;
    }
}