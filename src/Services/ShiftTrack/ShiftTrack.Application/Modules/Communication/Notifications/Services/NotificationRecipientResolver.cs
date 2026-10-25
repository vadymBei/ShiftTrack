using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Services;

/// <summary>
/// Finds approvers the same way the approval strategies check access:
/// by the approver's role and the department / unit they belong to.
/// </summary>
public class NotificationRecipientResolver(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository) : INotificationRecipientResolver
{
    public Task<IReadOnlyCollection<long>> GetDepartmentDirectors(long departmentId,
        CancellationToken cancellationToken)
    {
        return employeeRepository.GetIdsByRole(
            DefaultRolesCatalog.DEPARTMENT_DIRECTOR, departmentId, null, cancellationToken);
    }

    public Task<IReadOnlyCollection<long>> GetUnitDirectors(long unitId, CancellationToken cancellationToken)
    {
        return employeeRepository.GetIdsByRole(
            DefaultRolesCatalog.UNIT_DIRECTOR, null, unitId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<long>> GetUnitDirectorsForDepartments(IEnumerable<long> departmentIds,
        CancellationToken cancellationToken)
    {
        var ids = departmentIds.Distinct().ToList();

        if (ids.Count == 0)
            return [];

        var departments = await departmentRepository.GetByIds(ids, cancellationToken);

        var unitIds = departments
            .Where(x => x.UnitId is not null)
            .Select(x => x.UnitId.Value)
            .Distinct();

        var result = new HashSet<long>();

        foreach (var unitId in unitIds)
        {
            result.UnionWith(await GetUnitDirectors(unitId, cancellationToken));
        }

        return result;
    }
}
