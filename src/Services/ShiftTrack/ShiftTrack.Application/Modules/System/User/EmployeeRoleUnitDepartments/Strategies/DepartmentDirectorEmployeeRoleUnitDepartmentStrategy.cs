using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;

public class DepartmentDirectorEmployeeRoleUnitDepartmentStrategy(
    ICurrentUserService currentUserService,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository,
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentRepository)
    : IEmployeeRoleUnitDepartmentStrategy
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(
        EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository
            .GetById(dto.EmployeeRoleUnitId, cancellationToken);

        if (employeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.UnitId
                              && u.Departments.Any(d =>
                                  d.DepartmentId == employeeRoleUnit.EmployeeRole.Employee.DepartmentId))))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var employeeRoleUnitDepartments = await employeeRoleUnitDepartmentRepository.Create(
            dto,
            cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task Delete(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await employeeRoleUnitDepartmentRepository.GetById(
            employeeRoleUnitDepartmentId,
            cancellationToken);

        if (employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog
                .DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        var employeeRole = employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole;

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department.UnitId
                              && u.Departments.Any(d => d.DepartmentId == employeeRole.Employee.DepartmentId))))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleUnitDepartmentRepository.Delete(employeeRoleUnitDepartmentId, cancellationToken);
    }
}