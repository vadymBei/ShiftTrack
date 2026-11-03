using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.EditMessage;

public class EditMessageCommandHandler(
    IMessageService messageService) : IRequestHandler<EditMessageCommand, MessageVm>
{
    public async Task<MessageVm> Handle(EditMessageCommand request, CancellationToken cancellationToken = default)
    {
        return (await messageService.Edit(request.Id, request.Text, cancellationToken)).ToVm();
    }
}
