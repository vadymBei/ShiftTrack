using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Data.Extensions;

namespace ShiftTrack.Core.Application.System.User.Roles.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleVM>>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(
            IMapper mapper, 
            IMemoryCache memoryCache,
            IRoleService roleService)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _roleService = roleService;
        }

        public async Task<IEnumerable<RoleVM>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _memoryCache.GetOrCreateAsync(
                "roles",
                async () =>
                {
                    return await _roleService
                        .GetRoles(cancellationToken);
                });


            return _mapper.Map<List<RoleVM>>(roles);
        }
    }
}
