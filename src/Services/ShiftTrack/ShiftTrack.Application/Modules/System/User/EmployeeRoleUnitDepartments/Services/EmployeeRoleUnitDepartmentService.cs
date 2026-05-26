using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Services;

public class EmployeeRoleUnitDepartmentService(
    IEmployeeRoleUnitRepository employeeRoleUnitRepository,
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentRepository,
    IEmployeeRoleUnitDepartmentStrategyFactory employeeRoleUnitDepartmentStrategyFactory)
    : IEmployeeRoleUnitDepartmentService
{
    private IEmployeeRoleUnitDepartmentStrategy Strategy
        => employeeRoleUnitDepartmentStrategyFactory.GetStrategy();

    public async Task<EmployeeRoleUnitDepartment> Create(
        EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
        await employeeRoleUnitDepartmentRepository.CheckIfExists(dto, cancellationToken);

        var employeeRoleUnitDepartments = await Strategy
            .Create(dto, cancellationToken);

        await employeeRoleUnitRepository.RecalculateScope(dto.EmployeeRoleUnitId, cancellationToken);

        return employeeRoleUnitDepartments.FirstOrDefault();
    }

    public async Task Delete(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await employeeRoleUnitDepartmentRepository.GetById(
            employeeRoleUnitDepartmentId,
            cancellationToken);

        var employeeRoleUnitId = employeeRoleUnitDepartment.EmployeeRoleUnitId;

        await Strategy.Delete(
            employeeRoleUnitDepartmentId,
            cancellationToken);

        if (employeeRoleUnitId is not null)
        {
            await employeeRoleUnitRepository.RecalculateScope((long)employeeRoleUnitId, cancellationToken);
        }
    }
}