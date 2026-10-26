using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Models;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Services;

public class DepartmentService(
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IDepartmentRepository departmentRepository,
    IRoleScopeService roleScopeService)
    : IDepartmentService
{
    public async Task<IEnumerable<GroupedDepartmentsByUnit>> GetGroupedDepartmentsByUnit(
        CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAll(cancellationToken);

        var units = await unitRepository.GetAll(cancellationToken);

        var groupedDepartments = new List<GroupedDepartmentsByUnit>();

        foreach (var unit in units)
        {
            var unitDepartments = departments
                .Where(x => x.UnitId == unit.Id);

            groupedDepartments.Add(new GroupedDepartmentsByUnit()
            {
                Unit = unit,
                Departments = unitDepartments.OrderBy(x => x.Name).ToList()
            });
        }

        groupedDepartments = groupedDepartments
            .OrderBy(x => x.Unit.Name)
            .ToList();

        return groupedDepartments;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByRoles(long unitId, CancellationToken cancellationToken)
    {
        var departments = new List<Department>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            departments = (await departmentRepository.GetAll(cancellationToken)).ToList();
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            departments = await GetDepartmentsByScope(DefaultRolesCatalog.UNIT_DIRECTOR, unitId, cancellationToken);
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            departments = await GetDepartmentsByScope(DefaultRolesCatalog.DEPARTMENT_DIRECTOR, unitId,
                cancellationToken);
        }
        else if (currentUserService.Employee.Department is not null)
        {
            if (currentUserService.Employee.Department.UnitId != unitId)
                return departments;

            var department = await departmentRepository.GetById(
                (long)currentUserService.Employee.DepartmentId!,
                cancellationToken);

            departments = [department];
        }

        return departments;
    }

    /// <summary>Departments of the unit the director manages by the role bindings.</summary>
    private async Task<List<Department>> GetDepartmentsByScope(string roleName, long unitId,
        CancellationToken cancellationToken)
    {
        var scope = roleScopeService.GetCurrentScope(roleName);

        if (!scope.IsBoundTo(unitId))
            return [];

        return (await departmentRepository.GetDepartmentsByUnitId(unitId, cancellationToken))
            .Where(x => scope.Covers(unitId, x.Id))
            .ToList();
    }
}
