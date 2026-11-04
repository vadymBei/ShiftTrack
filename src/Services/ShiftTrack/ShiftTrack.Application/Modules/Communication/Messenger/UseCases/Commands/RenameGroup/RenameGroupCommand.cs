using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RenameGroup;

public record RenameGroupCommand(
    long Id,
    string Title) : IRequest<ConversationVm>;
