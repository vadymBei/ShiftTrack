using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetAttachment;

public record GetAttachmentQuery(
    long Id,
    bool Download) : IRequest<AttachmentFileVm>;
