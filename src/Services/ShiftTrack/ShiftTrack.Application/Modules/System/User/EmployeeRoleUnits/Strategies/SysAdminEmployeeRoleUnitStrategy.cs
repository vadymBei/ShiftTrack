using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Strategies;

public class SysAdminEmployeeRoleUnitStrategy(
    IEmployeeRoleUnitRepository employeeRoleUnitRepository)
    : IEmployeeRoleUnitStrategy
{
    public async Task<EmployeeRoleUnit> Create(
        EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.Create(dto, cancellationToken);

        return employeeRoleUnit;
    }

    public async Task Delete(long employeeUnitId, CancellationToken cancellationToken)
    {
        await employeeRoleUnitRepository.Delete(employeeUnitId, cancellationToken);
    }
}