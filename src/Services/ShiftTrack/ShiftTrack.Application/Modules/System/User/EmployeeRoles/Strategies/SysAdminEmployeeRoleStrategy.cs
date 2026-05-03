using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Strategies;

public sealed class SysAdminEmployeeRoleStrategy(
    IEmployeeRoleRepository employeeRoleRepository) : IEmployeeRoleStrategy
{
    public async Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.Create(dto, cancellationToken);

        return employeeRole;
    }

    public async Task Delete(long employeeRoleId, CancellationToken cancellationToken)
    {
        await employeeRoleRepository.Delete(employeeRoleId, cancellationToken);
    }
}