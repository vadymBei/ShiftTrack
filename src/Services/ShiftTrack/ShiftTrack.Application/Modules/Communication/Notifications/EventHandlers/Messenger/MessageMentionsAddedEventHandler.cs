using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Events;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.Messenger;

/// <summary>
/// A notification for every mentioned member, also in a muted group: a mention is addressed to the person.
/// </summary>
public class MessageMentionsAddedEventHandler(
    ILogger<MessageMentionsAddedEventHandler> logger,
    IMessageRepository messageRepository,
    IConversationRepository conversationRepository,
    INotificationService notificationService) : NotificationEventHandler<MessageMentionsAddedEvent>(logger)
{
    protected override async Task HandleEvent(MessageMentionsAddedEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = await messageRepository.GetById(domainEvent.MessageId, cancellationToken);

        if (message is null || message.IsDeleted)
            return;

        // Who left the group or was removed by now doesn't get it.
        var members = await conversationRepository.GetMemberIds(message.ConversationId, cancellationToken);
        var recipients = domainEvent.EmployeeIds
            .Where(id => id != message.AuthorId && members.Contains(id))
            .ToList();

        if (recipients.Count == 0)
            return;

        var text = MessengerMapper.PlainText(message) ?? string.Empty;
        if (text.Length > MessengerConstants.MentionNotificationLength)
            text = text[..MessengerConstants.MentionNotificationLength] + "…";

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.Mention,
                NotificationModuleType.Messenger,
                message.ConversationId,
                SystemMessageTexts.Mentioned(message.Author, message.Conversation?.Title ?? "групі"),
                text),
            recipients,
            cancellationToken);
    }
}
