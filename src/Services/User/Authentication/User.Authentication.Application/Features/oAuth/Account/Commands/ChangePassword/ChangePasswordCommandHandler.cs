using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Account.Commands.ChangePassword;

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