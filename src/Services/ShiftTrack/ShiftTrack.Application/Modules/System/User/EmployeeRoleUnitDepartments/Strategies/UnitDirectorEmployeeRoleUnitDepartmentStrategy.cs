using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;

public class UnitDirectorEmployeeRoleUnitDepartmentStrategy(
    IEmployeeRoleUnitRepository employeeRoleUnitRepository, 
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentRepository,
    IRoleScopeService roleScopeService) : IEmployeeRoleUnitDepartmentStrategy
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.GetById(dto.EmployeeRoleUnitId, cancellationToken);
        
        var scope = roleScopeService.GetCurrentScope(DefaultRolesCatalog.UNIT_DIRECTOR);

        if (!dto.DepartmentIds.All(departmentId => scope.Covers(employeeRoleUnit.UnitId, departmentId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var employeeRoleUnitDepartments = await employeeRoleUnitDepartmentRepository.Create(dto, cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task Delete(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await employeeRoleUnitDepartmentRepository.GetById(employeeRoleUnitDepartmentId, cancellationToken);
        
        if (!roleScopeService.GetCurrentScope(DefaultRolesCatalog.UNIT_DIRECTOR).Covers(
                employeeRoleUnitDepartment.EmployeeRoleUnit.UnitId,
                employeeRoleUnitDepartment.DepartmentId))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleUnitDepartmentRepository.Delete(employeeRoleUnitDepartmentId, cancellationToken);
    }
}