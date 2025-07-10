using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Employees.Commands.UpdateEmployee;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployeeById;
using ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployees;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Employees;

[Authorize]
[Route("api/shift-track/organization/employees")]
public class ORG_EMP_EmployeesController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<EmployeeVm>> GetEmployees([FromQuery] GetEmployeesQuery query)
        => await Mediator.Invoke(query);

    [HttpGet("{id}")]
    public async Task<EmployeeVm> GetEmployeeById(long id)
        => await Mediator.Invoke(new GetEmployeeByIdQuery(id));
    
    [HttpPut]
    public async Task<EmployeeVm> UpdateEmployee(UpdateEmployeeCommand command)
        => await Mediator.Invoke(command);
}