using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;

namespace ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;

/// <summary>
/// The single source of what a director manages: the role bindings to units and departments.
/// </summary>
public interface IRoleScopeService
{
    /// <summary>Scope of the current employee in the role.</summary>
    RoleScope GetCurrentScope(string roleName);

    /// <summary>Departments covered by the current employee's role, whole units expanded to their departments.</summary>
    Task<IReadOnlySet<long>> GetCurrentDepartmentIds(string roleName, CancellationToken cancellationToken);

    /// <summary>Units touched by the current employee's role: whole units and units of separate departments.</summary>
    Task<IReadOnlySet<long>> GetCurrentUnitIds(string roleName, CancellationToken cancellationToken);
}
