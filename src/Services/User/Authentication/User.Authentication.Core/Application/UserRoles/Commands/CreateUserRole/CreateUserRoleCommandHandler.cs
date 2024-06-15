using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Application.UserRoles.Commands.CreateUserRole
{
    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand>
    {
        private readonly IUserRoleService _userRoleService;

        public CreateUserRoleCommandHandler(
            IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        public async Task<Unit> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userRoleService
                .CreateUserRole(request.Data);

            return Unit.Value;
        }
    }
}
