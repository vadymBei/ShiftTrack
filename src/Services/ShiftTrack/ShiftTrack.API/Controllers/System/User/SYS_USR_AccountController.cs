using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateAccount;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/account")]
public class SYS_USR_AccountController: ApiController
{
    [HttpGet("current-user")]
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
    public async Task<EmployeeVM> UpdateAccount (UpdateAccountCommand command)
        => await Mediator.Invoke(command);
}