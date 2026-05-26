using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Strategies;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Services;

public class EmployeeRoleService(
    IEnumerable<IEmployeeRoleStrategy> strategies,
    IEmployeeRoleRepository employeeRoleRepository,
    IEmployeeRoleStrategyFactory employeeRoleStrategyFactory) : IEmployeeRoleService
{
    private IEmployeeRoleStrategy EmployeeRoleStrategy => employeeRoleStrategyFactory.GetStrategy();

    public async Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        await employeeRoleRepository.CheckIfExists(dto, cancellationToken);

        return await EmployeeRoleStrategy.Create(dto, cancellationToken);
    }

    public Task Delete(long employeeRoleId, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.Delete(employeeRoleId, cancellationToken);
    }

    public Task<EmployeeRole> CreateSysAdminEmployeeRole(EmployeeRoleToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var sysAdminStrategy = strategies.OfType<SysAdminEmployeeRoleStrategy>().First();

        return sysAdminStrategy.Create(dto, cancellationToken);
    }
}