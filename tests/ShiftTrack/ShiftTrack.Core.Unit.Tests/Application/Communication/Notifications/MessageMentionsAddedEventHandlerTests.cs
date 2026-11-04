using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Events;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.Messenger;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Notifications;

public class MessageMentionsAddedEventHandlerTests
{
    private readonly Mock<IMessageRepository> _messages = new();
    private readonly Mock<IConversationRepository> _conversations = new();
    private readonly Mock<INotificationService> _notifications = new();
    private readonly MessageMentionsAddedEventHandler _handler;

    public MessageMentionsAddedEventHandlerTests()
    {
        _conversations.Setup(x => x.GetMemberIds(10, It.IsAny<CancellationToken>())).ReturnsAsync([1L, 2L, 3L]);
        _handler = new MessageMentionsAddedEventHandler(NullLogger<MessageMentionsAddedEventHandler>.Instance,
            _messages.Object, _conversations.Object, _notifications.Object);
    }

    private void Given(Message message) =>
        _messages.Setup(x => x.GetById(message.Id, It.IsAny<CancellationToken>())).ReturnsAsync(message);

    [Fact]
    public async Task MentionedMembers_GetANotificationWithTheText()
    {
        Given(new Message
        {
            Id = 5, ConversationId = 10, AuthorId = 1, Type = MessageType.Text,
            Author = ConversationBuilder.Employee(1, "Коваль", "Олена"),
            Conversation = new Conversation { Id = 10, Type = ConversationType.Group, Title = "Нічна зміна" },
            Text = "<@2>, вийдеш завтра?",
            Mentions = [new MessageMention { EmployeeId = 2, Employee = ConversationBuilder.Employee(2, "Шевчук", "Ірина") }]
        });

        await _handler.Handle(new MessageMentionsAddedEvent(5, [2, 9]), CancellationToken.None);

        _notifications.Verify(x => x.Notify(
            It.Is<NotificationToCreateDto>(n => n.Type == NotificationType.Mention
                && n.ModuleType == NotificationModuleType.Messenger
                && n.EntityId == 10
                && n.Title == "Коваль Олена  згадала вас у «Нічна зміна»"
                && n.Message == "@Шевчук Ірина , вийдеш завтра?"),
            It.Is<IEnumerable<long>>(r => r.SequenceEqual(new long[] { 2 })),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletedMessage_NotifiesNobody()
    {
        Given(new Message { Id = 5, ConversationId = 10, AuthorId = 1, DeletedAt = DateTime.UtcNow });

        await _handler.Handle(new MessageMentionsAddedEvent(5, [2]), CancellationToken.None);

        _notifications.Verify(x => x.Notify(It.IsAny<NotificationToCreateDto>(), It.IsAny<IEnumerable<long>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
