using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.DeleteMessage;

public record DeleteMessageCommand(
    long Id) : IRequest<MessageVm>;
