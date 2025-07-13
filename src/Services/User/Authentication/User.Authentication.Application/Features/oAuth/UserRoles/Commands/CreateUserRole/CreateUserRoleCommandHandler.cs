using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;

namespace User.Authentication.Application.Features.oAuth.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandHandler(
    IUserRoleService userRoleService) : IRequestHandler<CreateUserRoleCommand>
{
    public async Task Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        await userRoleService
            .CreateUserRole(request.Data);
    }
}