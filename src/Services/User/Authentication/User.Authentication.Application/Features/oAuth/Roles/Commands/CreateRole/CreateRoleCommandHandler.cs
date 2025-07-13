using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler(
    IMapper mapper,
    IRoleService roleService) : IRequestHandler<CreateRoleCommand, RoleVm>
{
    public async Task<RoleVm> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleService
            .CreateRole(request.Data);

        return mapper.Map<RoleVm>(role);
    }
}