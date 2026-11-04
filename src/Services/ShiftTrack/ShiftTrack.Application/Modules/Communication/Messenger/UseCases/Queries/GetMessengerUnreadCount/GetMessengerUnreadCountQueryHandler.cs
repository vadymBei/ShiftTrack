using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessengerUnreadCount;

public class GetMessengerUnreadCountQueryHandler(
    IConversationService conversationService) : IRequestHandler<GetMessengerUnreadCountQuery, MessengerUnreadCountVm>
{
    public async Task<MessengerUnreadCountVm> Handle(GetMessengerUnreadCountQuery request, CancellationToken cancellationToken = default)
    {
        var count = await conversationService.CountUnread(cancellationToken);

        return new MessengerUnreadCountVm { Conversations = count.Conversations, Messages = count.Messages };
    }
}
