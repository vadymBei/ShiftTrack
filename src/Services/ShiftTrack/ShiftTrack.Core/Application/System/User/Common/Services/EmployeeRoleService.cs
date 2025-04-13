using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class EmployeeRoleService(
    IEmployeeRoleStrategyFactory employeeRoleStrategyFactory) : IEmployeeRoleService
{
    private IEmployeeRoleStrategy EmployeeRoleStrategy
        => employeeRoleStrategyFactory.GetStrategy();

    public Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.CreateEmployeeRole(dto, cancellationToken);
    }

    public Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.DeleteEmployeeRole(employeeRoleId, cancellationToken);
    }

    public Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.GetEmployeeRoleById(employeeRoleId, cancellationToken);
    }

    public Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(long employeeId, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.GetEmployeeRolesByEmployeeId(employeeId, cancellationToken);
    }
}