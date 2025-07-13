using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Roles.Queries.GetRoles;

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