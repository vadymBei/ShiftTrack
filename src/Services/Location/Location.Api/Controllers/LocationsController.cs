using Location.Application.Common.ViewModels;
using Location.Application.UseCases.Commands.SearchLocations;
using Location.Application.UseCases.Queries.GetLocationsByIntegrationIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace Location.Api.Controllers;

[Authorize]
[Route("api/locations")]
public class LocationsController : ApiController
{
    [HttpPost("search")]
    public Task<IEnumerable<LocationVm>> Search([FromBody] SearchLocationsCommand command)
        => Mediator.Invoke(command);
    
    [HttpGet("by-integrationIds")]
    public Task<IEnumerable<LocationVm>> GetLocationsByIntegrationIds([FromQuery] GetLocationsByIntegrationIdsQuery query)
        => Mediator.Invoke(query);
}