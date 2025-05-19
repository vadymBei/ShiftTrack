using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployees;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employees")]
public class SYS_USR_EmployeesController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<EmployeeVM>> GetEmployees([FromQuery] GetEmployeesQuery query)
        => await Mediator.Invoke(query);

    [HttpGet("{id}")]
    public async Task<EmployeeVM> GetEmployeeById(long id)
        => await Mediator.Invoke(new GetEmployeeByIdQuery(id));
    
    [HttpPut]
    public async Task<EmployeeVM> UpdateEmployee(UpdateEmployeeCommand command)
        => await Mediator.Invoke(command);
}