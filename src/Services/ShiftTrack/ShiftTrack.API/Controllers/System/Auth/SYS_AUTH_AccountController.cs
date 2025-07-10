using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.Register;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.UpdateAccount;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.UploadPhoto;
using ShiftTrack.Application.Features.System.Auth.Account.Queries.GetCurrentUser;
using ShiftTrack.Application.Features.System.Auth.Account.Queries.GetUserPhoto;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Application.Features.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[Authorize]
[Route("api/shift-track/system/auth/account")]
public class SYS_AUTH_AccountController : ApiController
{
    [HttpGet("current-user")]
    public Task<CurrentUserVm> GetCurrentUser()
        => Mediator.Invoke(new GetCurrentUserQuery());

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<TokenVm> CreateEmployee(RegisterCommand command)
    {
        var employee = await Mediator.Invoke(command);

        return await Mediator.Invoke(
            new GenerateTokenCommand(
                new GenerateTokenDto(command.PhoneNumber, command.Password)));
    }

    [HttpPost("password/change")]
    public async Task<TokenVm> ChangePassword(ChangePasswordDto commandData)
        => await Mediator.Invoke(new ChangePasswordCommand(commandData));
    
    [HttpPut]
    public Task<EmployeeVm> UpdateAccount(UpdateAccountCommand command)
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