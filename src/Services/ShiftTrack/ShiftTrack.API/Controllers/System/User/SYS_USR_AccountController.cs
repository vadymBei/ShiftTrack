using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateAccount;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UploadPhoto;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetUserPhoto;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/account")]
public class SYS_USR_AccountController : ApiController
{
    [HttpGet("current-user")]
    public Task<CurrentUserVM> GetCurrentUser()
        => Mediator.Invoke(new GetCurrentUserQuery());

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
    public Task<EmployeeVM> UpdateAccount(UpdateAccountCommand command)
        => Mediator.Invoke(command);

    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoCommand command)
    {
        var document = await Mediator.Invoke(command);

        return PhysicalFile(document.Path, document.GetMimeType());
    }

    [HttpGet("photo")]
    public async Task<IActionResult> GetPhoto([FromQuery] GetUserPhotoQuery query)
    {
        var document = await Mediator.Invoke(query);

        return PhysicalFile(document.Path, document.GetMimeType());
    }
}