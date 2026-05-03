using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.ViewModels;
using ShiftTrack.Data.Extensions;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.Roles.UseCases.Queries.GetRoles;

public class GetRolesQueryHandler(
    IMapper mapper,
    IMemoryCache memoryCache,
    IRoleRepository roleRepository)
    : IRequestHandler<GetRolesQuery, IEnumerable<RoleVm>>
{
    public async Task<IEnumerable<RoleVm>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await memoryCache.GetOrCreateAsync(
            "roles",
            async () => await roleRepository
                .GetAll(cancellationToken));

        return mapper.Map<List<RoleVm>>(roles);
    }
}