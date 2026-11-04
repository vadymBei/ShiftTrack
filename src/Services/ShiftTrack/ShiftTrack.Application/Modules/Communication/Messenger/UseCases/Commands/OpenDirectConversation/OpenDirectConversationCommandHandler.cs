using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.OpenDirectConversation;

public class OpenDirectConversationCommandHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<OpenDirectConversationCommand, ConversationVm>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<ConversationVm> Handle(OpenDirectConversationCommand request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.OpenDirect(request.EmployeeId, cancellationToken)).ToVm(Me);
    }
}
