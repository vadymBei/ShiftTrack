using AutoMapper;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;

[AutoMap(typeof(Notification))]
public class NotificationVm
{
    public long Id { get; set; }
    public NotificationType Type { get; set; }
    public NotificationModuleType ModuleType { get; set; }
    public long? EntityId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public long? AuthorId { get; set; }
    public string AuthorFullName { get; set; }
}
