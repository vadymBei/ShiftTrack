using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Services;

public class EmployeeRoleUnitService(
    IEmployeeRoleRepository employeeRoleRepository,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository,
    IEmployeeRoleUnitStrategyFactory employeeRoleUnitStrategyFactory)
    : IEmployeeRoleUnitService
{
    private IEmployeeRoleUnitStrategy Strategy
        => employeeRoleUnitStrategyFactory.GetStrategy();

    public async Task<EmployeeRoleUnit> Create(
        EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
        await employeeRoleUnitRepository.CheckIfExists(dto, cancellationToken);

        var employeeRole = await employeeRoleRepository.GetById(dto.EmployeeRoleId, cancellationToken);

        var employeeRoleUnit = await Strategy.Create(dto, cancellationToken);

        await employeeRoleRepository.RecalculateScope(employeeRole.Id, cancellationToken);

        return employeeRoleUnit;
    }

    public async Task Delete(long employeeRoleUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.GetById(employeeRoleUnitId, cancellationToken);

        var employeeRoleId = employeeRoleUnit.EmployeeRoleId;

        await Strategy.Delete(employeeRoleUnitId, cancellationToken);

        if (employeeRoleId is not null)
        {
            await employeeRoleRepository.RecalculateScope((long)employeeRoleId, cancellationToken);
        }
    }
}