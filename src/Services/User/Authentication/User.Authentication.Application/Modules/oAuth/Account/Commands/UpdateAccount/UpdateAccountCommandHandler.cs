using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<UpdateAccountCommand, UserVm>
{
    public async Task<UserVm> Handle(UpdateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var user = await userService.UpdateUser(
            new UserToUpdateDto(
                request.Id,
                request.Email,
                request.PhoneNumber));

        return mapper.Map<UserVm>(user);
    }
}