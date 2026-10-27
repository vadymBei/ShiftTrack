using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.DeleteNotification;

public record DeleteNotificationCommand(
    long Id) : IRequest;
