using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.EditMessage;

public record EditMessageCommand(
    long Id,
    string Text) : IRequest<MessageVm>;
