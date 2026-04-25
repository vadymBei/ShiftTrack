using AutoMapper;
using Location.Application.Common.Interfaces;
using Location.Application.Common.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Data.Extensions;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Commands.SearchLocations;

public class SearchLocationsCommandHandler(
    IMapper mapper,
    IMemoryCache memoryCache,
    ILocationService locationService) : IRequestHandler<SearchLocationsCommand, IEnumerable<LocationVm>>
{
    public async Task<IEnumerable<LocationVm>> Handle(SearchLocationsCommand request, CancellationToken cancellationToken = default)
    {
        var locations = await memoryCache.GetOrCreateAsync(
            request.SearchTerm,
            async () => await locationService.Search(request.SearchTerm, cancellationToken));
        
        return mapper.Map<IEnumerable<LocationVm>>(locations);
    }
}