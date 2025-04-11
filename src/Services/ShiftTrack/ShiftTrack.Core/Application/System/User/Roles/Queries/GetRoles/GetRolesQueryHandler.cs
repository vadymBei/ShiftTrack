using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Data.Extensions;

namespace ShiftTrack.Core.Application.System.User.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(
    IMapper mapper,
    IMemoryCache memoryCache,
    IRoleService roleService)
    : IRequestHandler<GetRolesQuery, IEnumerable<RoleVM>>
{
    public async Task<IEnumerable<RoleVM>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await memoryCache.GetOrCreateAsync(
            "roles",
            async () => await roleService
                .GetRoles(cancellationToken));

        return mapper.Map<List<RoleVM>>(roles);
    }
}