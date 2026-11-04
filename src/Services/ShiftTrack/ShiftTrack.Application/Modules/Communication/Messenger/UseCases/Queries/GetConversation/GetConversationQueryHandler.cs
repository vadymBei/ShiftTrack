using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetConversation;

public class GetConversationQueryHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<GetConversationQuery, ConversationVm>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<ConversationVm> Handle(GetConversationQuery request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.Get(request.Id, cancellationToken)).ToVm(Me);
    }
}
