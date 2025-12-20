using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Common.Services;

public class CommonVacationService(
    IShiftService shiftService,
    IEmployeeShiftService employeeShiftService,
    IApplicationDbContext applicationDbContext) : ICommonVacationService
{
    public IQueryable<Vacation> GetVacationsQuery(VacationsFilterDto filter)
    {
        var vacationsQuery = applicationDbContext.Vacations
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.Employee.Department.UnitId == filter.UnitId
                        && x.Employee.DepartmentId == filter.DepartmentId);

        if (filter.StartDate is not null
            && filter.EndDate is not null)
        {
            vacationsQuery = vacationsQuery.Where(x => x.StartDate >= filter.StartDate
                                                       && x.StartDate <= filter.EndDate);
        }

        if (filter.Status is not VacationStatus.None)
        {
            vacationsQuery = vacationsQuery.Where(x => x.Status == filter.Status);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
        {
            vacationsQuery = vacationsQuery
                .Where(x => EF.Functions.Like(
                    x.Employee.Surname.ToLower() + " " + x.Employee.Name.ToLower() + " " +
                    x.Employee.Patronymic.ToLower(),
                    $"%{filter.SearchPattern.ToLower()}%"));
        }

        return vacationsQuery;
    }

    public async Task<Vacation> GetVacationById(long vacationId, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations
                           .AsNoTracking()
                           .Include(x => x.Employee)
                           .ThenInclude(x => x.Department)
                           .FirstOrDefaultAsync(x => x.Id == vacationId, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), vacationId);

        return vacation;
    }

    public async Task<Vacation> GetVacationForStatusChange(long vacationId, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations
                           .Include(x => x.Employee)
                           .ThenInclude(x => x.Department)
                           .FirstOrDefaultAsync(x => x.Id == vacationId, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), vacationId);

        return vacation;
    }

    public async Task SetVacationShifts(long vacationId, CancellationToken cancellationToken)
    {
        var vacation = await GetVacationById(vacationId, cancellationToken);

        var startDate = vacation.StartDate;
        var endDate = vacation.EndDate;
        var daysCount = (endDate - startDate).Days + 1;

        var vacationShift = await shiftService.GetShiftByCode(ShiftCodes.AnnualLeave, cancellationToken);

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

    public async Task RestoreEmployeeShiftsBeforeVacation(long vacationId, CancellationToken cancellationToken)
    {
        var vacation = await GetVacationById(vacationId, cancellationToken);
        
        await employeeShiftService.RestorePreviousEmployeeShifts(
            new RestoreEmployeeShiftsDto(
                [vacation.EmployeeId],
                vacation.StartDate,
                vacation.EndDate),
            cancellationToken);
    }
}