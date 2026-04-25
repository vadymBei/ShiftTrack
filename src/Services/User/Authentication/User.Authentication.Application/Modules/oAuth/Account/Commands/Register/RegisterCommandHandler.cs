using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.Register;

public class RegisterCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<RegisterCommand, UserVm>
{
    public async Task<UserVm> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        var result = await userService.CreateUser(
            new UserToCreateDto
            (
                request.Email,
                request.PhoneNumber,
                request.Password
            ));

        return mapper.Map<UserVm>(result);
    }
}