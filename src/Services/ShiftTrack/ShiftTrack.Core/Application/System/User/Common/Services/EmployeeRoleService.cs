using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class EmployeeRoleService(
    IUserRoleRepository userRoleRepository) : IEmployeeRoleService
{
    public async Task CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        await userRoleRepository.CreateUserRole(
            new UserRoleToCreateDto(
                dto.EmployeeIntegrationId,
                dto.RoleId)
            , cancellationToken);
    }
}