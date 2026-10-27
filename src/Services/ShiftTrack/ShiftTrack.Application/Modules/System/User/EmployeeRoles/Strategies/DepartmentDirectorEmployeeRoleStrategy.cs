using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Strategies;

public sealed class DepartmentDirectorEmployeeRoleStrategy(
    IRoleRepository roleRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleRepository employeeRoleRepository,
    IRoleScopeService roleScopeService) : IEmployeeRoleStrategy
{
    public async Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        if (dto.UnitId is null
            || !dto.DepartmentIds.Any())
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CANNOT_ASSIGN_GLOBAL_SCOPE,
                nameof(UserExceptionsLocalization.CANNOT_ASSIGN_GLOBAL_SCOPE));
        }

        var employee = await employeeRepository.GetById(dto.EmployeeId, cancellationToken);

        var scope = roleScopeService.GetCurrentScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

        // The employee works in the director's scope and the granted departments are in it too.
        if (!scope.Covers(employee)
            || !dto.DepartmentIds.All(departmentId => scope.Covers(dto.UnitId, departmentId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_DEPARTMENT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_DEPARTMENT_OUT_OF_SCOPE));
        }

        var role = await roleRepository.GetById(dto.RoleId, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        var employeeRole = await employeeRoleRepository.Create(dto, cancellationToken);

        return employeeRole;
    }

    public async Task Delete(long employeeRoleId, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.GetById(employeeRoleId, cancellationToken);

        var role = await roleRepository.GetById((long)employeeRole.RoleId!, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (!roleScopeService.GetCurrentScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR).Covers(employeeRole.Employee))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT));
        }

        await employeeRoleRepository.Delete(employeeRole.Id, cancellationToken);
    }
}