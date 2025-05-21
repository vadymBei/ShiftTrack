using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(
    IMapper mapper,
    IRoleService roleService) : IRequestHandler<GetRolesQuery, IEnumerable<RoleVM>>
{
    public async Task<IEnumerable<RoleVM>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleService
            .GetRoles(cancellationToken);

        return mapper.Map<List<RoleVM>>(roles);
    }
}