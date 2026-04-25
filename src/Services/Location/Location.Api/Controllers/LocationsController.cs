using Location.Application.Common.ViewModels;
using Location.Application.UseCases.Commands.SearchLocations;
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
}