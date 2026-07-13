using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeeById;
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

    [HttpGet("{id}")]
    public async Task<EmployeeVm> GetEmployeeById(long id)
        => await Mediator.Send(new GetEmployeeByIdQuery(id));
    
    [HttpPut]
    public async Task<EmployeeVm> UpdateEmployee(UpdateEmployeeCommand command)
        => await Mediator.Send(command);
}