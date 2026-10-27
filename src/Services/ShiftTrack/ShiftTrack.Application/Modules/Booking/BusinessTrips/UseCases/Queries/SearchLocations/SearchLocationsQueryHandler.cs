using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.SearchLocations;

public class SearchLocationsQueryHandler(
    IMapper mapper,
    ILocationRepository locationRepository) : IRequestHandler<SearchLocationsQuery, IEnumerable<LocationVm>>
{
    public async Task<IEnumerable<LocationVm>> Handle(SearchLocationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var locations = await locationRepository.Search(
            request.SearchTerm.Trim(), 
            cancellationToken);

        return mapper.Map<IEnumerable<LocationVm>>(locations);
    }
}
