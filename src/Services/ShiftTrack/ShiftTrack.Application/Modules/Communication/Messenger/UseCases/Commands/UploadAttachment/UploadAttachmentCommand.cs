using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.UploadAttachment;

public record UploadAttachmentCommand(
    long ConversationId,
    AttachmentUpload File) : IRequest<AttachmentVm>;
