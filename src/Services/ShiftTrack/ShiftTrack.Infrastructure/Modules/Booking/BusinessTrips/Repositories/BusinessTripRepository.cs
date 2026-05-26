using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
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
        CancellationToken cancellationToken)
    {
        var businessTripQuery = applicationDbContext.BusinessTrips
            .AsNoTracking()
            .Include(x => x.Locations)
            .Include(x => x.Participants)
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

    public async Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await applicationDbContext.BusinessTrips
                               .Include(x => x.Locations)
                               .Include(x => x.Participants)
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