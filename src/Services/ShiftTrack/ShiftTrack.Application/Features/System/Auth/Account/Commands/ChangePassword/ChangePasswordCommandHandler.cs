using AutoMapper;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IAccountService accountService) : IRequestHandler<ChangePasswordCommand, TokenVm>
{
    public async Task<TokenVm> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await accountService
            .ChangePassword(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}