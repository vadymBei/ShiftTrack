using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Extensions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTripById;

public class GetBusinessTripByIdQueryHandler(
    IMapper mapper,
    ILocationRepository locationRepository,
    IBusinessTripService businessTripService) : IRequestHandler<GetBusinessTripByIdQuery, BusinessTripVm>
{
    public async Task<BusinessTripVm> Handle(GetBusinessTripByIdQuery request, CancellationToken cancellationToken = default)
    {
        var businessTrip = await businessTripService.GetById(request.Id, cancellationToken);
        
        var locations = await locationRepository
            .GetListByIds(businessTrip.Locations.Select(x => x.LocationIntegrationId), cancellationToken);
        
        var businessTripVm = mapper.Map<BusinessTripVm>(businessTrip);
        businessTripVm.EnrichWithLocationData(locations
            .ToDictionary(x => x.IntegrationId));
        
        return businessTripVm;
    }
}