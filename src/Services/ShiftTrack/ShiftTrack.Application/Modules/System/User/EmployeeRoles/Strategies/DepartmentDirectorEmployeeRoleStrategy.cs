using ShiftTrack.Application.Common.Interfaces;
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
    ICurrentUserService currentUserService,
    IEmployeeRoleRepository employeeRoleRepository) : IEmployeeRoleStrategy
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

        var currentUser = currentUserService.Employee;

        if (employee.Department is null
            || currentUser.Department.UnitId != dto.UnitId
            || !dto.DepartmentIds.All(x => x == currentUser.DepartmentId)
            || !currentUser.EmployeeRoles.Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                                                   && x.Units.Any(u => u.Departments.Any(d =>
                                                       dto.DepartmentIds.Contains((long)d.DepartmentId)))
                                                   && x.Units.Any(u => u.UnitId == employee.Department.UnitId))
           )
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

        if (employeeRole.Employee.Department is null
            || employeeRole.Employee.Department.Id != currentUserService.Employee.DepartmentId
            || employeeRole.Employee.Department.UnitId != currentUserService.Employee.Department.UnitId)
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT));
        }

        await employeeRoleRepository.Delete(employeeRole.Id, cancellationToken);
    }
}