using AutoMapper;
using Location.Application.Common.Interfaces;
using Location.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Queries.GetLocationsByIntegrationIds;

public class GetLocationsByIntegrationIdsQueryHandler(
    IMapper mapper,
    ILocationRepository locationRepository) : IRequestHandler<GetLocationsByIntegrationIdsQuery, IEnumerable<LocationVm>>
{
    public async Task<IEnumerable<LocationVm>> Handle(GetLocationsByIntegrationIdsQuery request, CancellationToken cancellationToken = default)
    {
        var locations = await locationRepository
            .GetByIntegrationIds(request.IntegrationIds, cancellationToken);
        
        return mapper.Map<IEnumerable<LocationVm>>(locations);
    }
}