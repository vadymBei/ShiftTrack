using ShiftTrack.Application.Modules.Communication.Messenger.Events;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using System.Net;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

public class MessageService(
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository,
    IMessageRateLimiter messageRateLimiter,
    IMessengerPublisher messengerPublisher,
    IMessageAttachmentRepository attachmentRepository,
    IAttachmentStorage attachmentStorage,
    IDomainEventOutbox domainEventOutbox) : IMessageService
{
    private long MyId => currentUserService.Employee.Id;

    public async Task<IReadOnlyList<Message>> GetPage(long conversationId, long? beforeId, int take,
        CancellationToken cancellationToken)
    {
        if (!await conversationRepository.IsMember(conversationId, MyId, cancellationToken))
            throw NotAMember();

        return await messageRepository.GetPage(conversationId, beforeId,
            Math.Clamp(take, 1, MessengerConstants.MaxMessagesPage), cancellationToken);
    }

    public async Task<Message> Send(long conversationId, string text, Guid? clientMessageId, long? replyToMessageId,
        IReadOnlyCollection<long> attachmentIds, CancellationToken cancellationToken)
    {
        var conversation = await conversationRepository.GetForUpdate(conversationId, cancellationToken);

        if (conversation?.FindMember(MyId) is null)
            throw NotAMember();

        // A retry of a request that has already been saved.
        if (clientMessageId is not null)
        {
            var sent = await messageRepository.FindByClientId(MyId, clientMessageId.Value, cancellationToken);

            if (sent is not null && sent.ConversationId == conversationId)
                return sent;
        }

        if (conversation.Type == ConversationType.Direct && conversation.Members.Count < 2)
            throw Error(MessengerExceptionsLocalization.RECIPIENT_UNAVAILABLE,
                nameof(MessengerExceptionsLocalization.RECIPIENT_UNAVAILABLE));

        if (replyToMessageId is not null
            && !await messageRepository.Exists(conversationId, replyToMessageId.Value, cancellationToken))
            throw Error(MessengerExceptionsLocalization.MESSAGE_FROM_OTHER_CONVERSATION,
                nameof(MessengerExceptionsLocalization.MESSAGE_FROM_OTHER_CONVERSATION));

        var attachments = await GetAttachmentsToSend(conversationId, attachmentIds, cancellationToken);

        if (!messageRateLimiter.TryAcquire(MyId))
            throw Error(MessengerExceptionsLocalization.TOO_MANY_MESSAGES,
                nameof(MessengerExceptionsLocalization.TOO_MANY_MESSAGES), HttpStatusCode.TooManyRequests);

        var message = new Message
        {
            ConversationId = conversationId,
            Type = MessageType.Text,
            Text = string.IsNullOrWhiteSpace(text) ? null : text.Trim(),
            ClientMessageId = clientMessageId,
            ReplyToMessageId = replyToMessageId,
            Attachments = attachments.ToList()
        };

        var mentioned = MentionedMembers(conversation, message.Text);
        message.Mentions = mentioned.Select(x => new MessageMention { EmployeeId = x }).ToList();

        await messageRepository.Add(message, cancellationToken);

        if (mentioned.Count > 0)
            await domainEventOutbox.Enqueue(new MessageMentionsAddedEvent(message.Id, mentioned), cancellationToken);
        await conversationRepository.SetLastMessage(conversationId, message.Id, message.CreatedAt, cancellationToken);
        await conversationRepository.MarkRead(conversationId, MyId, message.Id, cancellationToken);

        var saved = await messageRepository.GetById(message.Id, cancellationToken);
        var memberIds = conversation.Members.Select(x => x.EmployeeId).ToList();
        var messageVm = saved.ToVm();

        // Also to the sender's other tabs; a new direct chat appears for the other member with this message.
        await unitOfWork.AfterCommit(() => messengerPublisher.MessageCreated(memberIds, messageVm, CancellationToken.None));

        return saved;
    }

    public async Task<Message> Edit(long messageId, string text, CancellationToken cancellationToken)
    {
        var message = await GetOwn(messageId, cancellationToken);

        text = text.Trim();

        if (message.Text == text)
            return message;

        message.Text = text;
        message.EditedAt = DateTime.UtcNow;

        // Removed mentions go, only the added ones are notified.
        var conversation = await conversationRepository.GetForUpdate(message.ConversationId, cancellationToken);
        var mentioned = MentionedMembers(conversation, text);
        var added = mentioned.Where(id => message.Mentions.All(x => x.EmployeeId != id)).ToList();

        message.Mentions.RemoveAll(x => !mentioned.Contains(x.EmployeeId));
        message.Mentions.AddRange(added.Select(x => new MessageMention { MessageId = message.Id, EmployeeId = x }));

        await messageRepository.Save(cancellationToken);

        if (added.Count > 0)
            await domainEventOutbox.Enqueue(new MessageMentionsAddedEvent(message.Id, added), cancellationToken);

        var saved = await messageRepository.GetById(message.Id, cancellationToken);
        await PublishUpdated(saved, cancellationToken);

        return saved;
    }

    public async Task<Message> Delete(long messageId, CancellationToken cancellationToken)
    {
        var message = await GetOwn(messageId, cancellationToken);

        message.DeletedAt = DateTime.UtcNow;
        message.Text = null;

        // A deleted message keeps no files: they are removed from the storage after the commit.
        var storedNames = message.Attachments.Select(x => x.StoredName).ToList();
        if (message.Attachments.Count > 0)
        {
            await attachmentRepository.Remove(message.Attachments.ToList(), cancellationToken);
            message.Attachments.Clear();
        }

        await messageRepository.Save(cancellationToken);

        await unitOfWork.AfterCommit(() =>
        {
            storedNames.ForEach(attachmentStorage.Delete);
            return Task.CompletedTask;
        });
        await PublishUpdated(message, cancellationToken);

        return message;
    }

    /// <summary>
    /// Members of a group mentioned in the text, except the author. Mentions are a group thing:
    /// in a direct chat the other member gets every message anyway.
    /// </summary>
    private IReadOnlyList<long> MentionedMembers(Conversation conversation, string text)
    {
        if (conversation?.Type != ConversationType.Group)
            return [];

        return MentionTokens.Parse(text)
            .Where(id => id != MyId && conversation.FindMember(id) is not null)
            .Take(MessengerConstants.MaxMentionsPerMessage)
            .ToList();
    }

    private async Task<IReadOnlyList<MessageAttachment>> GetAttachmentsToSend(long conversationId,
        IReadOnlyCollection<long> attachmentIds, CancellationToken cancellationToken)
    {
        var ids = (attachmentIds ?? []).Distinct().ToList();

        if (ids.Count == 0)
            return [];

        if (ids.Count > MessengerConstants.MaxAttachmentsPerMessage)
            throw Error(MessengerExceptionsLocalization.TOO_MANY_ATTACHMENTS,
                nameof(MessengerExceptionsLocalization.TOO_MANY_ATTACHMENTS));

        var attachments = await attachmentRepository.GetUnsent(MyId, conversationId, ids, cancellationToken);

        if (attachments.Count != ids.Count)
            throw Error(MessengerExceptionsLocalization.ATTACHMENTS_NOT_FOUND,
                nameof(MessengerExceptionsLocalization.ATTACHMENTS_NOT_FOUND));

        return attachments;
    }

    private async Task<Message> GetOwn(long messageId, CancellationToken cancellationToken)
    {
        var message = await messageRepository.GetForUpdate(messageId, cancellationToken)
                      ?? throw new EntityNotFoundException(typeof(Message), messageId);

        if (!await conversationRepository.IsMember(message.ConversationId, MyId, cancellationToken))
            throw NotAMember();

        if (message.AuthorId != MyId)
            throw Error(MessengerExceptionsLocalization.CANNOT_CHANGE_OTHERS_MESSAGE,
                nameof(MessengerExceptionsLocalization.CANNOT_CHANGE_OTHERS_MESSAGE), HttpStatusCode.Forbidden);

        if (message.Type != MessageType.Text || message.IsDeleted)
            throw Error(MessengerExceptionsLocalization.CANNOT_CHANGE_MESSAGE,
                nameof(MessengerExceptionsLocalization.CANNOT_CHANGE_MESSAGE));

        return message;
    }

    private async Task PublishUpdated(Message message, CancellationToken cancellationToken)
    {
        var memberIds = await conversationRepository.GetMemberIds(message.ConversationId, cancellationToken);
        var messageVm = message.ToVm();

        await unitOfWork.AfterCommit(() => messengerPublisher.MessageUpdated(memberIds, messageVm, CancellationToken.None));
    }

    private static MessengerException NotAMember() =>
        Error(MessengerExceptionsLocalization.NOT_A_MEMBER, nameof(MessengerExceptionsLocalization.NOT_A_MEMBER),
            HttpStatusCode.Forbidden);

    private static MessengerException Error(string message, string type, HttpStatusCode code = HttpStatusCode.BadRequest) =>
        new(message, type, code);
}
