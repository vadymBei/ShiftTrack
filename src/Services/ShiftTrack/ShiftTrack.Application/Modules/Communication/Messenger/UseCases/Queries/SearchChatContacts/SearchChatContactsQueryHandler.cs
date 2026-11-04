using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchChatContacts;

public class SearchChatContactsQueryHandler(
    IConversationService conversationService) : IRequestHandler<SearchChatContactsQuery, List<ChatContactVm>>
{
    public async Task<List<ChatContactVm>> Handle(SearchChatContactsQuery request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.SearchContacts(request.Search, cancellationToken)).Select(x => x.ToContactVm()).ToList();
    }
}
