using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeeById;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhoto;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhotos;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployees;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Employees;

[Authorize]
[Route("api/shift-track/organization/employees")]
public class ORG_EMP_EmployeesController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<EmployeeVm>> GetEmployees([FromQuery] EmployeesFilterDto filter)
        => await Mediator.Send(new GetEmployeesQuery(filter));

    [HttpGet("{id:long}")]
    public async Task<EmployeeVm> GetEmployeeById(long id)
        => await Mediator.Send(new GetEmployeeByIdQuery(id));

    /// <summary>Which of the employees have a photo; the version goes to the photo URL so that it can be cached.</summary>
    [HttpGet("photos")]
    public Task<IReadOnlyList<EmployeePhotoVm>> GetPhotos([FromQuery] long[] ids)
        => Mediator.Send(new GetEmployeePhotosQuery(ids ?? []));

    /// <summary>The photo of the employee, 404 without one. Cached for long: a new photo has a new version (?v=).</summary>
    [HttpGet("{id:long}/photo")]
    public async Task<IActionResult> GetPhoto(long id)
    {
        var photo = await Mediator.Send(new GetEmployeePhotoQuery(id));

        Response.Headers["X-Content-Type-Options"] = "nosniff";
        Response.Headers.CacheControl = Request.Query.ContainsKey("v")
            ? "private, max-age=31536000, immutable"
            : "private, no-cache";

        return File(photo.Content, photo.ContentType);
    }
    
    [HttpPut]
    public async Task<EmployeeVm> UpdateEmployee(UpdateEmployeeCommand command)
        => await Mediator.Send(command);
}