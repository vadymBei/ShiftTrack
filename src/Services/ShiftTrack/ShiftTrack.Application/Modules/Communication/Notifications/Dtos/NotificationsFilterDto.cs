namespace ShiftTrack.Application.Modules.Communication.Notifications.Dtos;

public record NotificationsFilterDto(
    int Take,
    bool OnlyUnread);
