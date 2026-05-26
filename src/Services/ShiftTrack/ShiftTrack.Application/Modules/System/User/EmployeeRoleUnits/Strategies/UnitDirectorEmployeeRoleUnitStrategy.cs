using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Strategies;

public class UnitDirectorEmployeeRoleUnitStrategy(
    ICurrentUserService currentUserService,
    IEmployeeRoleRepository employeeRoleRepository,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository) : IEmployeeRoleUnitStrategy
{
    public async Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.GetById(dto.EmployeeRoleId, cancellationToken);

        if (employeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department
                                  .UnitId)))
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
        
        if (employeeRoleUnit.EmployeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.EmployeeRole.Employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        await employeeRoleUnitRepository.Delete(employeeUnitId, cancellationToken);
    }
}