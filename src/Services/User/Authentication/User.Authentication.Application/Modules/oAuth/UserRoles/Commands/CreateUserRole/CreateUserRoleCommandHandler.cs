using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;

namespace User.Authentication.Application.Modules.oAuth.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandHandler(
    IUserRoleService userRoleService) : IRequestHandler<CreateUserRoleCommand>
{
    public async Task Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        await userRoleService
            .CreateUserRole(request.Data);
    }
}