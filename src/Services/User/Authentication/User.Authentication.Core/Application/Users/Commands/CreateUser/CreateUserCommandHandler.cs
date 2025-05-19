using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<CreateUserCommand, UserVM>
{
    public async Task<UserVM> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userService.CreateUser(
            new UserToCreateDto
            (
                request.Email,
                request.PhoneNumber,
                request.Password
            ));

        return mapper.Map<UserVM>(result);
    }
}