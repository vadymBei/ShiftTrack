namespace ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;

/// <summary>Approvers of a department, found by the role bindings (the same source the strategies use).</summary>
public interface INotificationRecipientResolver
{
    Task<IReadOnlyCollection<long>> GetDepartmentDirectors(long departmentId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<long>> GetUnitDirectors(long departmentId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<long>> GetUnitDirectorsForDepartments(IEnumerable<long> departmentIds,
        CancellationToken cancellationToken);
}
