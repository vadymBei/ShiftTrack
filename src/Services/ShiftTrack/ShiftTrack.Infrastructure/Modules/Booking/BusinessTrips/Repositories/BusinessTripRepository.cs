using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Repositories;

public class BusinessTripRepository(
    IApplicationDbContext applicationDbContext) : IBusinessTripRepository
{
    public async Task<BusinessTrip> Create(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        try
        {
            await applicationDbContext.BusinessTrips.AddAsync(businessTrip, cancellationToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
        return businessTrip;
    }
}