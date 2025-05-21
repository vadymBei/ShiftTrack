using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler(
    IMapper mapper,
    IRoleService roleService) : IRequestHandler<CreateRoleCommand, RoleVM>
{
    public async Task<RoleVM> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleService
            .CreateRole(request.Data);

        return mapper.Map<RoleVM>(role);
    }
}