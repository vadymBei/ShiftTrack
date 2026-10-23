using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class SysAdminVacationStrategy(
    ICurrentUserService currentUserService,
    IVacationRepository vacationRepository,
    IEmployeeShiftService employeeShiftService,
    IVacationShiftService vacationShiftService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        await vacationRepository.UpdateVacationStatus(
            new UpdateVacationStatusDto(
                vacation.Id,
                VacationStatus.ApprovedByUnitDirector),
            cancellationToken);

        await vacationShiftService.SetVacationShifts(id, cancellationToken);
    }

    public async Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

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

        return vacation;
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector],
            null,
            null,
            currentUserService.Employee.Id);
    }

    public async Task<IEnumerable<Vacation>> GetVacations(
        VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacations = await vacationRepository.GetFiltered(filter, cancellationToken);

        return vacations;
    }
}