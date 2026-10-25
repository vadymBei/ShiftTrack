using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Communication.Notifications.Entities;

public class Notification : AuditableEntity
{
    public long Id { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public NotificationType Type { get; set; }
    public NotificationModuleType ModuleType { get; set; }
    public long? EntityId { get; set; }

    public string Title { get; set; }
    public string Message { get; set; }

    public DateTime? ReadAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
