using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Booking.Vacations.Repositories;

public class VacationRepository(
    IApplicationDbContext applicationDbContext) : IVacationRepository
{
    public async Task<Vacation> Create(VacationToCreateDto vacationToCreateDto, CancellationToken cancellationToken)
    {
        var vacation = new Vacation()
        {
            StartDate = vacationToCreateDto.StartDate,
            EndDate = vacationToCreateDto.EndDate,
            EmployeeId = vacationToCreateDto.EmployeeId,
            Comment = vacationToCreateDto.Comment,
            Type = vacationToCreateDto.Type,
            DaysBalanceAtCreation = vacationToCreateDto.EmployeeVacationDaysBalance,
            DaysCount = (vacationToCreateDto.EndDate - vacationToCreateDto.StartDate).Days + 1
        };

        await applicationDbContext.Vacations.AddAsync(vacation, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return vacation;
    }

    public async Task<Vacation> GetById(long id, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations
                           .AsNoTracking()
                           .Include(x => x.Employee)
                           .ThenInclude(x => x.Department)
                           .Include(x => x.Employee)
                           .ThenInclude(x => x.Position)
                           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), id);

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken)
    {
        var vacationsQuery = applicationDbContext.Vacations
            .AsNoTracking()
            .Include(x => x.Author)
            .Include(x => x.Employee)
            .ThenInclude(x => x.Department)
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

        return await vacationsQuery.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vacation>> GetByEmployeeOrAuthor(long employeeId, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Vacations
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId || x.AuthorId == employeeId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vacation>> GetForApproval(
        VacationApprovalFilterDto filter,
        int take,
        CancellationToken cancellationToken)
    {
        return await ApplyApprovalFilter(filter)
            .Include(x => x.Employee)
            .ThenInclude(x => x.Position)
            .OrderBy(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountForApproval(VacationApprovalFilterDto filter, CancellationToken cancellationToken)
    {
        return ApplyApprovalFilter(filter).CountAsync(cancellationToken);
    }

    private IQueryable<Vacation> ApplyApprovalFilter(VacationApprovalFilterDto filter)
    {
        var query = applicationDbContext.Vacations
            .AsNoTracking()
            .Where(x => filter.Statuses.Contains(x.Status));

        if (filter.UnitIds is not null || filter.DepartmentIds is not null)
        {
            var unitIds = filter.UnitIds ?? [];
            var departmentIds = filter.DepartmentIds ?? [];

            query = query.Where(x =>
                (x.Employee.Department.UnitId != null && unitIds.Contains(x.Employee.Department.UnitId.Value))
                || (x.Employee.DepartmentId != null && departmentIds.Contains(x.Employee.DepartmentId.Value)));
        }

        if (filter.ExcludeEmployeeId is not null)
            query = query.Where(x => x.EmployeeId != filter.ExcludeEmployeeId);

        return query;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations
                           .FindAsync([id], cancellationToken: cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), id);

        applicationDbContext.Vacations.Remove(vacation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Vacation> Update(Vacation vacation, CancellationToken cancellationToken)
    {
        applicationDbContext.Vacations.Update(vacation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return vacation;
    }

    public async Task<IReadOnlyList<Vacation>> GetApproved(IReadOnlyCollection<VacationType> types,
        long? employeeId, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Vacations
            .AsNoTracking()
            .Where(x => x.Status == VacationStatus.ApprovedByUnitDirector
                        && types.Contains(x.Type));

        if (employeeId is not null)
            query = query.Where(x => x.EmployeeId == employeeId);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Vacation> GetForUpdate(long id, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Vacations
                   .Include(x => x.Employee)
                   .ThenInclude(x => x.Department)
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
               ?? throw new EntityNotFoundException(typeof(Vacation), id);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}