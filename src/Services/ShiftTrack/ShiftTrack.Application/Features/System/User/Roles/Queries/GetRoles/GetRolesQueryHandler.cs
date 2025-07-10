using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Data.Extensions;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(
    IMapper mapper,
    IMemoryCache memoryCache,
    IRoleService roleService)
    : IRequestHandler<GetRolesQuery, IEnumerable<RoleVm>>
{
    public async Task<IEnumerable<RoleVm>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await memoryCache.GetOrCreateAsync(
            "roles",
            async () => await roleService
                .GetRoles(cancellationToken));

        return mapper.Map<List<RoleVm>>(roles);
    }
}