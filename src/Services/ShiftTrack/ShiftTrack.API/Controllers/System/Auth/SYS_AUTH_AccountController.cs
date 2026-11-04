using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.ChangePassword;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.Register;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UpdateAccount;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Queries.GetCurrentUser;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.RemovePhoto;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;
using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.System.Auth.Account.ViewModels;
using ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.GenerateToken;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[Authorize]
[Route("api/shift-track/system/auth/account")]
public class SYS_AUTH_AccountController : ApiController
{
    [HttpGet("current-user")]
    public Task<CurrentUserVm> GetCurrentUser()
        => Mediator.Send(new GetCurrentUserQuery());

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<TokenVm> CreateEmployee(RegisterCommand command)
    {
        var employee = await Mediator.Send(command);

        return await Mediator.Send(
            new GenerateTokenCommand(
                new GenerateTokenDto(command.PhoneNumber, command.Password)));
    }

    [HttpPost("password/change")]
    public async Task<TokenVm> ChangePassword(ChangePasswordDto commandData)
        => await Mediator.Send(new ChangePasswordCommand(commandData));
    
    [HttpPut]
    public Task<EmployeeVm> UpdateAccount(UpdateAccountCommand command)
        => Mediator.Send(command);

    /// <summary>Replaces the photo of the current user; the photo is read at organization/employees/{id}/photo.</summary>
    [HttpPost("photo")]
    [RequestSizeLimit(EmployeePhotoConstants.MaxSize + 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = EmployeePhotoConstants.MaxSize + 1024 * 1024)]
    public async Task<EmployeePhotoVm> UploadPhoto(IFormFile file)
    {
        await using var content = file?.OpenReadStream() ?? Stream.Null;

        return await Mediator.Send(new UploadPhotoCommand(new PhotoUpload(content, file?.FileName, file?.Length ?? 0)));
    }

    [HttpDelete("photo")]
    public async Task<IActionResult> RemovePhoto()
    {
        await Mediator.Send(new RemovePhotoCommand());

        return NoContent();
    }
}