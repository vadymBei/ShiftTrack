using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Constants;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

public class VacationShiftService(
    IShiftRepository shiftRepository,
    IVacationRepository vacationRepository,
    IEmployeeShiftService employeeShiftService) : IVacationShiftService
{
    public async Task SetVacationShifts(long vacationId, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(vacationId, cancellationToken);

        var startDate = vacation.StartDate;
        var endDate = vacation.EndDate;
        var daysCount = (endDate - startDate).Days + 1;

        var vacationShift = await shiftRepository.GetShiftByCode(ShiftCodes.AnnualLeave, cancellationToken);

        var shiftsToCreate = Enumerable.Range(0, daysCount)
            .Select(offset => startDate.AddDays(offset))
            .Select(date => new EmployeeShiftToCreateDto
            {
                EmployeeId = vacation.EmployeeId,
                ShiftId = vacationShift.Id,
                Date = date
            })
            .ToList();

        await employeeShiftService.CreateEmployeeShifts(shiftsToCreate, cancellationToken);
    }
}