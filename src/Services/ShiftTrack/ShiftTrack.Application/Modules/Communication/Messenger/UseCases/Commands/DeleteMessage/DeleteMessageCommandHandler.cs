using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.DeleteMessage;

public class DeleteMessageCommandHandler(
    IMessageService messageService) : IRequestHandler<DeleteMessageCommand, MessageVm>
{
    public async Task<MessageVm> Handle(DeleteMessageCommand request, CancellationToken cancellationToken = default)
    {
        return (await messageService.Delete(request.Id, cancellationToken)).ToVm();
    }
}
