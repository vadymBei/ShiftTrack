using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Repositories;

public class BusinessTripRepository(
    IApplicationDbContext applicationDbContext) : IBusinessTripRepository
{
    public async Task<BusinessTrip> Create(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        await applicationDbContext.BusinessTrips.AddAsync(businessTrip, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return businessTrip;
    }

    public async Task<IEnumerable<BusinessTrip>> GetFiltered(BusinessTripFilterDto filter,
        BusinessTripAccessFilterDto access, CancellationToken cancellationToken)
    {
        var businessTripQuery = ApplyAccess(applicationDbContext.BusinessTrips.AsNoTracking(), access)
            .Include(x => x.Locations)
            .Include(x => x.Participants)
            .ThenInclude(x => x.Department)
            .Where(x => x.StartDate.Date >= filter.StartDate.Date
                        && x.EndDate.Date <= filter.EndDate.Date);

        if (filter.DepartmentId > 0)
        {
            businessTripQuery = businessTripQuery
                .Where(x => x.Participants.Any(p => p.DepartmentId == filter.DepartmentId));
        }

        if (!string.IsNullOrEmpty(filter.SearchPattern))
        {
            businessTripQuery = businessTripQuery
                .Where(x => x.Participants.Any(p =>
                    EF.Functions.Like(
                        p.Surname.ToLower() + " " + p.Name.ToLower() + " " + p.Patronymic.ToLower(),
                        $"%{filter.SearchPattern.ToLower()}%")));
        }

        return await businessTripQuery
            .OrderByDescending(x => x.StartDate)
            .ToListAsync(cancellationToken);
    }

    private static IQueryable<BusinessTrip> ApplyAccess(IQueryable<BusinessTrip> query,
        BusinessTripAccessFilterDto access)
    {
        if (access.IsUnrestricted)
            return query;

        var departmentIds = access.ParticipantDepartmentIds ?? [];
        var ownerId = access.OwnerId;

        return query.Where(x =>
            (ownerId != null && (x.AuthorId == ownerId || x.Participants.Any(p => p.Id == ownerId)))
            || x.Participants.Any(p => p.DepartmentId != null && departmentIds.Contains(p.DepartmentId.Value)));
    }

    public async Task<IEnumerable<BusinessTrip>> GetByParticipantOrAuthor(long employeeId,
        CancellationToken cancellationToken)
    {
        return await applicationDbContext.BusinessTrips
            .AsNoTracking()
            .Include(x => x.Locations)
            .Where(x => x.AuthorId == employeeId || x.Participants.Any(p => p.Id == employeeId))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BusinessTrip>> GetForApproval(
        BusinessTripApprovalFilterDto filter,
        int take,
        CancellationToken cancellationToken)
    {
        return await ApplyApprovalFilter(filter)
            .Include(x => x.Locations)
            .Include(x => x.Participants)
            .ThenInclude(x => x.Position)
            .OrderBy(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountForApproval(BusinessTripApprovalFilterDto filter, CancellationToken cancellationToken)
    {
        return ApplyApprovalFilter(filter).CountAsync(cancellationToken);
    }

    private IQueryable<BusinessTrip> ApplyApprovalFilter(BusinessTripApprovalFilterDto filter)
    {
        var query = applicationDbContext.BusinessTrips
            .AsNoTracking()
            .Where(x => x.Status == BusinessTripStatus.PendingApproval);

        if (filter.ParticipantDepartmentIds is not null)
        {
            var departmentIds = filter.ParticipantDepartmentIds;
            query = query.Where(x => x.Participants.Any(p => p.DepartmentId != null
                                                             && departmentIds.Contains(p.DepartmentId.Value)));
        }

        if (filter.ExcludeParticipantId is not null)
        {
            var excludeId = filter.ExcludeParticipantId.Value;
            query = query.Where(x => !x.Participants.Any(p => p.Id == excludeId));
        }

        return query;
    }

    public async Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await applicationDbContext.BusinessTrips
                               .Include(x => x.Author)
                               .ThenInclude(x => x.Department)
                               .ThenInclude(x => x.Unit)
                               .Include(x => x.Locations)
                               .Include(x => x.Participants)
                               .ThenInclude(x => x.Position)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(BusinessTrip), id);

        return businessTrip;
    }

    public async Task Update(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        applicationDbContext.BusinessTrips.Update(businessTrip);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await applicationDbContext.BusinessTrips
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(BusinessTrip), id);

        applicationDbContext.BusinessTrips.Remove(businessTrip);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}