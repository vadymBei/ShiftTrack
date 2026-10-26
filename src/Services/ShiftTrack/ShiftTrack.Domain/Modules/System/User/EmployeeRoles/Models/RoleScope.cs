using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;

/// <summary>
/// What an employee manages in a role, taken from the role bindings (EmployeeRoleUnit / EmployeeRoleUnitDepartment).
/// A unit binding without departments covers the whole unit, with departments only those departments.
/// The employee's own department is where they work and doesn't extend the scope.
/// </summary>
public sealed class RoleScope
{
    public static readonly RoleScope Empty = new(new HashSet<long>(), new HashSet<long>(), new HashSet<long>());

    private RoleScope(IReadOnlySet<long> unitIds, IReadOnlySet<long> departmentIds, IReadOnlySet<long> boundUnitIds)
    {
        UnitIds = unitIds;
        DepartmentIds = departmentIds;
        BoundUnitIds = boundUnitIds;
    }

    /// <summary>Whole units in the scope.</summary>
    public IReadOnlySet<long> UnitIds { get; }

    /// <summary>Separate departments in the scope.</summary>
    public IReadOnlySet<long> DepartmentIds { get; }

    /// <summary>Units that have a binding at all, completely or by some of their departments.</summary>
    public IReadOnlySet<long> BoundUnitIds { get; }

    public bool IsEmpty => UnitIds.Count == 0 && DepartmentIds.Count == 0;

    /// <summary>Scope of the employee in the role. Needs EmployeeRoles.Role / Units / Departments loaded.</summary>
    public static RoleScope Of(Employee employee, string roleName)
    {
        if (employee?.EmployeeRoles is null)
            return Empty;

        var unitIds = new HashSet<long>();
        var departmentIds = new HashSet<long>();
        var boundUnitIds = new HashSet<long>();

        var bindings = employee.EmployeeRoles
            .Where(x => x.Role?.Name == roleName)
            .SelectMany(x => x.Units ?? [])
            .Where(x => x.UnitId is not null);

        foreach (var binding in bindings)
        {
            boundUnitIds.Add(binding.UnitId!.Value);

            var departments = (binding.Departments ?? [])
                .Where(x => x.DepartmentId is not null)
                .Select(x => x.DepartmentId!.Value)
                .ToList();

            if (departments.Count == 0)
                unitIds.Add(binding.UnitId.Value);
            else
                departmentIds.UnionWith(departments);
        }

        return new RoleScope(unitIds, departmentIds, boundUnitIds);
    }

    public bool Covers(long? unitId, long? departmentId) =>
        (unitId is not null && UnitIds.Contains(unitId.Value))
        || (departmentId is not null && DepartmentIds.Contains(departmentId.Value));

    /// <summary>Whether the employee works in the scope. Needs <see cref="Employee.Department"/> loaded.</summary>
    public bool Covers(Employee employee) =>
        employee is not null && Covers(employee.Department?.UnitId, employee.DepartmentId);

    public bool IsBoundTo(long? unitId) => unitId is not null && BoundUnitIds.Contains(unitId.Value);
}
