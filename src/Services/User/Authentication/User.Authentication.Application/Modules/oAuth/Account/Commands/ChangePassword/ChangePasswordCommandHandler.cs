using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<ChangePasswordCommand, TokenVm>
{
    public async Task<TokenVm> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await userService
            .ChangePassword(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}