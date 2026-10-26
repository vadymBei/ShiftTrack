using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Services;

/// <summary>
/// Finds approvers the same way the approval strategies check access: by the role bindings to units and departments.
/// </summary>
public class NotificationRecipientResolver(
    IEmployeeRepository employeeRepository) : INotificationRecipientResolver
{
    public Task<IReadOnlyCollection<long>> GetDepartmentDirectors(long departmentId,
        CancellationToken cancellationToken)
    {
        return employeeRepository.GetIdsByRoleScope(
            DefaultRolesCatalog.DEPARTMENT_DIRECTOR, [departmentId], cancellationToken);
    }

    public Task<IReadOnlyCollection<long>> GetUnitDirectors(long departmentId, CancellationToken cancellationToken)
    {
        return employeeRepository.GetIdsByRoleScope(
            DefaultRolesCatalog.UNIT_DIRECTOR, [departmentId], cancellationToken);
    }

    public Task<IReadOnlyCollection<long>> GetUnitDirectorsForDepartments(IEnumerable<long> departmentIds,
        CancellationToken cancellationToken)
    {
        return employeeRepository.GetIdsByRoleScope(
            DefaultRolesCatalog.UNIT_DIRECTOR, departmentIds.Distinct().ToList(), cancellationToken);
    }
}
