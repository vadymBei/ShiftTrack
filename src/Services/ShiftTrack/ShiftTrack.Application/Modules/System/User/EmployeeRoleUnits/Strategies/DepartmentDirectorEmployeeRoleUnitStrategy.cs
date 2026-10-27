using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Strategies;

public class DepartmentDirectorEmployeeRoleUnitStrategy(
    IEmployeeRoleUnitRepository employeeRoleUnitRepository,
    IEmployeeRoleRepository employeeRoleRepository,
    IRoleScopeService roleScopeService)
    : IEmployeeRoleUnitStrategy
{
    public async Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.GetById(dto.EmployeeRoleId, cancellationToken);

        if (employeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        var scope = roleScopeService.GetCurrentScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

        if (!scope.Covers(employeeRole.Employee) || !scope.IsBoundTo(dto.UnitId))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var employeeRoleUnit = await employeeRoleUnitRepository.Create(dto, cancellationToken);

        return employeeRoleUnit;
    }

    public async Task Delete(long employeeUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.GetById(employeeUnitId, cancellationToken);

        if (employeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (!roleScopeService.GetCurrentScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR)
                .Covers(employeeRoleUnit.EmployeeRole.Employee))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleUnitRepository.Delete(employeeUnitId, cancellationToken);
    }
}