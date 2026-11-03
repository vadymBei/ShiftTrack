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
        return (await messageService.GetPage(request.ConversationId, request.BeforeId, request.Take, cancellationToken)).Select(x => x.ToVm()).ToList();
    }
}
