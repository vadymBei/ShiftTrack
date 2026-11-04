using System.Net;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

public class MessageAttachmentService(
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IConversationRepository conversationRepository,
    IMessageAttachmentRepository attachmentRepository,
    IAttachmentStorage attachmentStorage,
    ILogger<MessageAttachmentService> logger) : IMessageAttachmentService
{
    private const int MaxFileNameLength = 255;

    private long MyId => currentUserService.Employee.Id;

    public async Task<MessageAttachment> Upload(long conversationId, AttachmentUpload upload,
        CancellationToken cancellationToken)
    {
        if (!await conversationRepository.IsMember(conversationId, MyId, cancellationToken))
            throw NotAMember();

        if (upload.Length <= 0)
            throw Error(MessengerExceptionsLocalization.ATTACHMENT_EMPTY,
                nameof(MessengerExceptionsLocalization.ATTACHMENT_EMPTY));

        if (upload.Length > MessengerConstants.MaxAttachmentSize)
            throw Error(MessengerExceptionsLocalization.ATTACHMENT_TOO_LARGE,
                nameof(MessengerExceptionsLocalization.ATTACHMENT_TOO_LARGE), HttpStatusCode.RequestEntityTooLarge);

        var fileName = CleanFileName(upload.FileName);
        var extension = Path.GetExtension(fileName);

        // The type is taken from the extension, not from the browser, and decides how the file is served.
        if (!MessengerConstants.AttachmentTypes.TryGetValue(extension, out var contentType))
            throw Error(MessengerExceptionsLocalization.ATTACHMENT_TYPE_NOT_ALLOWED,
                nameof(MessengerExceptionsLocalization.ATTACHMENT_TYPE_NOT_ALLOWED));

        var storedName = await attachmentStorage.Save(upload.Content, extension.ToLowerInvariant(), cancellationToken);

        var attachment = new MessageAttachment
        {
            ConversationId = conversationId,
            FileName = fileName,
            StoredName = storedName,
            ContentType = contentType,
            Size = upload.Length
        };

        try
        {
            await attachmentRepository.Add(attachment, cancellationToken);
        }
        catch
        {
            attachmentStorage.Delete(storedName);
            throw;
        }

        return attachment;
    }

    public async Task RemoveUnsent(long id, CancellationToken cancellationToken)
    {
        var attachment = await attachmentRepository.GetById(id, cancellationToken);

        // Someone else's upload or a sent file looks the same as a missing one.
        if (attachment is null || attachment.AuthorId != MyId || attachment.MessageId is not null)
            throw NotFound();

        var unsent = await attachmentRepository.GetUnsent(MyId, attachment.ConversationId, [id], cancellationToken);
        await attachmentRepository.Remove(unsent, cancellationToken);

        await unitOfWork.AfterCommit(() =>
        {
            attachmentStorage.Delete(attachment.StoredName);
            return Task.CompletedTask;
        });
    }

    public async Task<AttachmentContent> Open(long id, CancellationToken cancellationToken)
    {
        var attachment = await attachmentRepository.GetById(id, cancellationToken)
                         ?? throw NotFound();

        if (!await conversationRepository.IsMember(attachment.ConversationId, MyId, cancellationToken))
            throw NotAMember();

        // Until it is sent, only the uploader sees the file.
        if (attachment.MessageId is null && attachment.AuthorId != MyId)
            throw NotFound();

        var content = attachmentStorage.Open(attachment.StoredName);

        if (content is null)
        {
            logger.LogError("{LogName} File {StoredName} of attachment {AttachmentId} is missing in the storage",
                "OpenAttachment", attachment.StoredName, attachment.Id);
            throw NotFound();
        }

        return new AttachmentContent(attachment, content);
    }

    /// <summary>Only the name of the file, without the path a browser may send, and without control characters.</summary>
    private static string CleanFileName(string fileName)
    {
        var name = Path.GetFileName((fileName ?? string.Empty).Replace('\\', '/'));
        name = new string(name.Where(c => !char.IsControl(c)).ToArray()).Trim();

        if (string.IsNullOrEmpty(name))
            name = "file";

        if (name.Length <= MaxFileNameLength)
            return name;

        var extension = Path.GetExtension(name);
        return name[..(MaxFileNameLength - extension.Length)] + extension;
    }

    private static MessengerException NotAMember() =>
        Error(MessengerExceptionsLocalization.NOT_A_MEMBER, nameof(MessengerExceptionsLocalization.NOT_A_MEMBER),
            HttpStatusCode.Forbidden);

    private static MessengerException NotFound() =>
        Error(MessengerExceptionsLocalization.ATTACHMENT_NOT_FOUND,
            nameof(MessengerExceptionsLocalization.ATTACHMENT_NOT_FOUND), HttpStatusCode.NotFound);

    private static MessengerException Error(string message, string type, HttpStatusCode code = HttpStatusCode.BadRequest) =>
        new(message, type, code);
}
