using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessages;

public class GetMessagesQueryHandler(
    IMessageService messageService) : IRequestHandler<GetMessagesQuery, List<MessageVm>>
{
    public async Task<List<MessageVm>> Handle(GetMessagesQuery request, CancellationToken cancellationToken = default)
    {
        var page = request.UntilId is { } untilId
            ? await messageService.GetPageUntil(request.ConversationId, request.BeforeId, untilId, request.Take, cancellationToken)
            : await messageService.GetPage(request.ConversationId, request.BeforeId, request.Take, cancellationToken);

        return page.Select(x => x.ToVm()).ToList();
    }
}
