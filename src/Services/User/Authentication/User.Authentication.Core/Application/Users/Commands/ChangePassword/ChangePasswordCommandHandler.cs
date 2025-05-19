using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<ChangePasswordCommand, TokenVM>
{
    public async Task<TokenVM> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await userService
            .ChangePassword(request.Data, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}