using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleVM>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(
            IMapper mapper,
            IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<IEnumerable<RoleVM>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleService
                .GetRoles(cancellationToken);

            return _mapper.Map<List<RoleVM>>(roles);
        }
    }
}
