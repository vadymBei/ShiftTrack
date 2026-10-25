namespace ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;

public interface INotificationRecipientResolver
{
    Task<IReadOnlyCollection<long>> GetDepartmentDirectors(long departmentId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<long>> GetUnitDirectors(long unitId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<long>> GetUnitDirectorsForDepartments(IEnumerable<long> departmentIds, CancellationToken cancellationToken);
}
