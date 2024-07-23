using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Common.Services
{
    public class EmployeeRoleService : IEmployeeRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public EmployeeRoleService(
            IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
        {
            await _userRoleRepository.CreateUserRole(
                new UserRoleToCreateDto(
                    dto.EmployeeIntegrationId,
                    dto.RoleId)
                , cancellationToken);
        }
    }
}
