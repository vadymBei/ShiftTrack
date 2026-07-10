using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Extensions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTrips;

public class GetBusinessTripsQueryHandler(
    IMapper mapper,
    ILocationRepository locationRepository,
    IBusinessTripService businessTripService) : IRequestHandler<GetBusinessTripsQuery, IEnumerable<BusinessTripVm>>
{
    public async Task<IEnumerable<BusinessTripVm>> Handle(GetBusinessTripsQuery request, CancellationToken cancellationToken = default)
    {
        var businessTrips = await businessTripService
            .GetFiltered(request.Filter, cancellationToken);
        
        if(!businessTrips.Any())
            return [];
        
        var locationIntegrationIds = businessTrips
            .SelectMany(x => x.Locations)
            .Select(x => x.LocationIntegrationId)
            .ToHashSet();
        
        var locations = await locationRepository
            .GetListByIds(locationIntegrationIds, cancellationToken);
        
        var locationsDictionary = locations
            .ToDictionary(x => x.IntegrationId);
        
        var businessTripVms = mapper.Map<IEnumerable<BusinessTripVm>>(businessTrips);

        foreach (var businessTripVm in businessTripVms)
        {
            businessTripVm.EnrichWithLocationData(locationsDictionary);
        }
        
        return businessTripVms;
    }
}