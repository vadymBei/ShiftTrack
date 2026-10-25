using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkAllNotificationsAsRead;

public record MarkAllNotificationsAsReadCommand : IRequest;
