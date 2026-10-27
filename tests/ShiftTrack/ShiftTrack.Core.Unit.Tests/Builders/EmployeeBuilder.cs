using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Domain.Modules.System.User.Roles.Entities;

namespace ShiftTrack.Core.Unit.Tests.Builders;

/// <summary>Employees with a department and role bindings, as loaded for the current user.</summary>
public class EmployeeBuilder
{
    private readonly Employee _employee = new() { Id = 1, EmployeeRoles = [] };

    public EmployeeBuilder WithId(long id)
    {
        _employee.Id = id;
        return this;
    }

    public EmployeeBuilder WorkingIn(long departmentId, long unitId)
    {
        _employee.DepartmentId = departmentId;
        _employee.Department = new Department { Id = departmentId, UnitId = unitId };
        return this;
    }

    /// <summary>Role bound to the whole unit.</summary>
    public EmployeeBuilder WithRoleInUnit(string roleName, long unitId) => WithRole(roleName, (unitId, []));

    /// <summary>Role bound to some departments of the unit.</summary>
    public EmployeeBuilder WithRoleInDepartments(string roleName, long unitId, params long[] departmentIds) =>
        WithRole(roleName, (unitId, departmentIds));

    public EmployeeBuilder WithRoleWithoutBindings(string roleName) => WithRole(roleName);

    private EmployeeBuilder WithRole(string roleName, params (long UnitId, long[] DepartmentIds)[] bindings)
    {
        _employee.EmployeeRoles.Add(new EmployeeRole
        {
            Role = new Role { Name = roleName },
            Units = bindings
                .Select(binding => new EmployeeRoleUnit
                {
                    UnitId = binding.UnitId,
                    Departments = binding.DepartmentIds
                        .Select(departmentId => new EmployeeRoleUnitDepartment { DepartmentId = departmentId })
                        .ToList()
                })
                .ToList()
        });

        return this;
    }

    public Employee Build() => _employee;
}
