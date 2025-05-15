using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword;
using ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployees;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employees")]
public class SYS_USR_EmployeesController : ApiController
{
    [HttpGet("current")]
    public async Task<CurrentUserVM> GetCurrentUser()
        => await Mediator.Invoke(new GetCurrentUserQuery());

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<TokenVM> CreateEmployee(CreateEmployeeCommand command)
    {
        var employee = await Mediator.Invoke(command);

        return await Mediator.Invoke(
            new GenerateTokenCommand(
                new GenerateTokenDto(command.PhoneNumber, command.Password)));
    }

    [HttpPut]
    public async Task<EmployeeVM> UpdateEmployee(UpdateEmployeeCommand command)
        => await Mediator.Invoke(command);

    [HttpGet]
    public async Task<IEnumerable<EmployeeVM>> GetEmployees([FromQuery] GetEmployeesQuery query)
        => await Mediator.Invoke(query);

    [HttpGet("{id}")]
    public async Task<EmployeeVM> GetEmployeeById(long id)
        => await Mediator.Invoke(new GetEmployeeByIdQuery(id));

    [HttpPost("change-password")]
    public async Task<TokenVM> ChangePassword(ChangeEmployeePasswordDto commandData)
        => await Mediator.Invoke(new ChangePasswordCommand(commandData));
}