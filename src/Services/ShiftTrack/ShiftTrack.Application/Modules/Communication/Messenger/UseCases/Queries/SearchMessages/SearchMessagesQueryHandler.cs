using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchMessages;

public class SearchMessagesQueryHandler(
    IMessageService messageService) : IRequestHandler<SearchMessagesQuery, MessageSearchVm>
{
    public async Task<MessageSearchVm> Handle(SearchMessagesQuery request, CancellationToken cancellationToken = default)
    {
        var result = await messageService.Search(request.ConversationId, request.Query, cancellationToken);

        return new MessageSearchVm { MessageIds = result.MessageIds.ToList(), HasMore = result.HasMore };
    }
}
