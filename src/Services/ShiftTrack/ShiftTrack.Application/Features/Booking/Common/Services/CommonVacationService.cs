using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Common.Services;

public class CommonVacationService(
    IApplicationDbContext applicationDbContext) : ICommonVacationService
{
    public IQueryable<Vacation> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken)
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
                           .ThenInclude(x=> x.Department)
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
}