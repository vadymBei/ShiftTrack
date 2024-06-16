using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Models;

namespace ShiftTrack.Core.Application.System.User.Common.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(
            IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
        {
           return _roleRepository.GetRoles(cancellationToken);
        }
    }
}
