using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(
    IMapper mapper,
    IRoleService roleService) : IRequestHandler<GetRolesQuery, IEnumerable<RoleVm>>
{
    public async Task<IEnumerable<RoleVm>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleService
            .GetRoles(cancellationToken);

        return mapper.Map<List<RoleVm>>(roles);
    }
}