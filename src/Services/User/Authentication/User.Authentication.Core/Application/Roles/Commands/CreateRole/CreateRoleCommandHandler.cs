using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleVM>
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public CreateRoleCommandHandler(
            IMapper mapper, 
            IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<RoleVM> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleService
                .CreateRole(request.Data);

            return _mapper.Map<RoleVM>(role);
        }
    }
}
