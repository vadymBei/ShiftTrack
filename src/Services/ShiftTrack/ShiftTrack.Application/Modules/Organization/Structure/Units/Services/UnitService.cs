using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Services;

public class UnitService(
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IDepartmentRepository departmentRepository)
    : IUnitService
{
    public async Task<IEnumerable<Unit>> GetUnitsByRoles(CancellationToken cancellationToken)
    {
        var currentUserEmployeeRoles = currentUserService.Employee.EmployeeRoles;

        var units = new List<Unit>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            units = (await unitRepository.GetAll(cancellationToken))
                .ToList();
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR);

            var unitIds = employeeRole.Units
                .Where(x => x.UnitId != null)
                .Select(x => (long)x.UnitId)
                .Distinct()
                .ToList();

            units = (await unitRepository.GetByIds(unitIds, cancellationToken))
                .ToList();
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

            var unitId = employeeRole.Units.FirstOrDefault()?.UnitId
                         ?? currentUserService.Employee.Department.UnitId;

            if (unitId is not null)
            {
                units =
                [
                    await unitRepository.GetById((long)unitId, cancellationToken)
                ];
            }
        }
        else if (currentUserService.Employee.Department is not null)
        {
            units =
            [
                await unitRepository.GetById((long)currentUserService.Employee.Department.UnitId!, cancellationToken)
            ];
        }

        return units;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await unitRepository.GetById(id, cancellationToken);
        
        await departmentRepository.DeleteByUnitId(id, cancellationToken);
        
        await unitRepository.Delete(id, cancellationToken);
    }
}