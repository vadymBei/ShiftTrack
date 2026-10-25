using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkNotificationAsRead;

public record MarkNotificationAsReadCommand(
    long Id) : IRequest;
