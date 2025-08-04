using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;
using User.Authentication.Application.Features.oAuth.Users.Queries.GetUsers;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/users")]
public class UsersController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<UserVm>> GetAllUsers()
        => await Mediator.Invoke(new GetUsersQuery());
}