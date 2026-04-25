using AutoMapper;
using Location.Application.Common.Interfaces;
using Location.Application.Common.ViewModels;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Commands.GetLocationsByIntegrationIds;

public class GetLocationsByIntegrationIdsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext) : IRequestHandler<GetLocationsByIntegrationIdsQuery, IEnumerable<LocationVm>>
{
    public async Task<IEnumerable<LocationVm>> Handle(GetLocationsByIntegrationIdsQuery request, CancellationToken cancellationToken = default)
    {
        var locations = await applicationDbContext.Locations
            .AsNoTracking()
            .Where(x => request.IntegrationIds.Contains(x.IntegrationId))
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IEnumerable<LocationVm>>(locations);
    }
}