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

public sealed class UnitDirectorEmployeeRoleStrategy(
    IRoleRepository roleRepository,
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleRepository employeeRoleRepository)
    : IEmployeeRoleStrategy
{
    public async Task<EmployeeRole> Create(
        EmployeeRoleToCreateDto dto,
        CancellationToken cancellationToken)
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
            || !currentUser.EmployeeRoles.Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                                                   && x.Units.Any(u => u.UnitId == employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var role = await roleRepository.GetById(dto.RoleId, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_DIRECTOR
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.UNIT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.UNIT_DIRECTOR_INVALID_ROLE));
        }

        var employeeRole = await employeeRoleRepository.Create(dto, cancellationToken);

        return employeeRole;
    }

    public async Task Delete(long employeeRoleId, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.GetById(employeeRoleId, cancellationToken);

        if (employeeRole.Employee.Department is null
            || employeeRole.Employee.Department.UnitId != currentUserService.Employee.Department.UnitId)
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleRepository.Delete(employeeRoleId, cancellationToken);
    }
}