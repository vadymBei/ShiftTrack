using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<UpdateUserCommand, UserVM>
{
    public async Task<UserVM> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.UpdateUser(
            new UserToUpdateDto(
                request.Id,
                request.Email,
                request.PhoneNumber));

        return mapper.Map<UserVM>(user);
    }
}