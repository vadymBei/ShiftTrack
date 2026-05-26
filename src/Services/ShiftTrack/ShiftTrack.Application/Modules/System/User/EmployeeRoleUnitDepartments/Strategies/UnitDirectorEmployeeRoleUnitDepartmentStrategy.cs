using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;

public class UnitDirectorEmployeeRoleUnitDepartmentStrategy(
    ICurrentUserService currentUserService,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository, 
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentRepository) : IEmployeeRoleUnitDepartmentStrategy
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.GetById(dto.EmployeeRoleUnitId, cancellationToken);
        
        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.UnitId)))
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
        
        var employeeRole = employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole;

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleUnitDepartmentRepository.Delete(employeeRoleUnitDepartmentId, cancellationToken);
    }
}