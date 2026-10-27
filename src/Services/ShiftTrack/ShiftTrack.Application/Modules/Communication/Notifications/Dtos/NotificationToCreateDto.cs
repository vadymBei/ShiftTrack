using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Dtos;

public record NotificationToCreateDto(
    NotificationType Type,
    NotificationModuleType ModuleType,
    long? EntityId,
    string Title,
    string Message,
    DateTime? ExpiresAt = null);
