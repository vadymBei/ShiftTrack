using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Application.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandHandler(IUserRoleService userRoleService) : IRequestHandler<CreateUserRoleCommand>
{
    public async Task Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        await userRoleService
            .CreateUserRole(request.Data);
    }
}