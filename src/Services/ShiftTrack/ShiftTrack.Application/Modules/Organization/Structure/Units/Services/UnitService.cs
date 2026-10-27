using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
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
    IDepartmentRepository departmentRepository,
    IRoleScopeService roleScopeService)
    : IUnitService
{
    public async Task<IEnumerable<Unit>> GetUnitsByRoles(CancellationToken cancellationToken)
    {
        var units = new List<Unit>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            units = (await unitRepository.GetAll(cancellationToken))
                .ToList();
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            units = await GetUnitsByScope(DefaultRolesCatalog.UNIT_DIRECTOR, cancellationToken);
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            units = await GetUnitsByScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR, cancellationToken);
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

    /// <summary>Units the director manages by the role bindings.</summary>
    private async Task<List<Unit>> GetUnitsByScope(string roleName, CancellationToken cancellationToken)
    {
        var unitIds = await roleScopeService.GetCurrentUnitIds(roleName, cancellationToken);

        if (unitIds.Count == 0)
            return [];

        return (await unitRepository.GetByIds(unitIds.ToList(), cancellationToken)).ToList();
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await unitRepository.GetById(id, cancellationToken);
        
        await departmentRepository.DeleteByUnitId(id, cancellationToken);
        
        await unitRepository.Delete(id, cancellationToken);
    }
}