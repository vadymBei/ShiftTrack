using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMyConversations;

public class GetMyConversationsQueryHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<GetMyConversationsQuery, List<ConversationVm>>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<List<ConversationVm>> Handle(GetMyConversationsQuery request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.GetMy(request.Take, cancellationToken)).Select(x => x.ToVm(Me)).ToList();
    }
}
