using System.Net;
using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class MessageServiceTests : MessengerFixture
{
    [Fact]
    public async Task Send_SavesTheTrimmedText_MovesTheChatUp_AndPushesAfterCommit()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        var pushedBeforeCommit = true;

        var message = await Command(async () =>
        {
            var sent = await MessageService.Send(10, "  Привіт  ", Guid.NewGuid(), null, [], default);
            pushedBeforeCommit = Publisher.Invocations.Count > 0;
            return sent;
        });

        Assert.False(pushedBeforeCommit);
        Assert.Equal("Привіт", message.Text);
        Assert.Equal(MessageType.Text, message.Type);
        Conversations.Verify(x => x.SetLastMessage(10, message.Id, message.CreatedAt, It.IsAny<CancellationToken>()), Times.Once);
        Conversations.Verify(x => x.MarkRead(10, Me, message.Id, It.IsAny<CancellationToken>()), Times.Once);
        Publisher.Verify(x => x.MessageCreated(Ids(Me, 2), It.Is<MessageVm>(m => m.Id == message.Id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Send_ARetry_ReturnsTheSavedMessage()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        var clientId = Guid.NewGuid();
        var saved = new Message { Id = 7, ConversationId = 10, ClientMessageId = clientId, Text = "Привіт" };
        Messages.Setup(x => x.FindByClientId(Me, clientId, default)).ReturnsAsync(saved);

        var message = await Command(() => MessageService.Send(10, "Привіт", clientId, null, [], default));

        Assert.Same(saved, message);
        Messages.Verify(x => x.Add(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Empty(Publisher.Invocations);
    }

    [Fact]
    public async Task Send_ToAChatYouAreNotIn_IsForbidden()
    {
        Existing(ConversationBuilder.Direct().WithMember(2).WithMember(3).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Send(10, "x", null, null, [], default));

        Assert.Equal("CMN_MSG_NOT_A_MEMBER", exception.ErrorType);
        Assert.Equal(HttpStatusCode.Forbidden, exception.Code);
    }

    [Fact]
    public async Task Send_ToAnUnavailableEmployee_IsRejected()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Send(10, "x", null, null, [], default));

        Assert.Equal("CMN_MSG_RECIPIENT_UNAVAILABLE", exception.ErrorType);
    }

    [Fact]
    public async Task Send_TooFast_IsRejected()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        RateLimiter.Setup(x => x.TryAcquire(Me)).Returns(false);

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Send(10, "x", null, null, [], default));

        Assert.Equal("CMN_MSG_TOO_MANY_MESSAGES", exception.ErrorType);
        Assert.Equal(HttpStatusCode.TooManyRequests, exception.Code);
    }

    [Fact]
    public async Task Reply_ToAMessageOfAnotherChat_IsRejected()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Send(10, "x", null, 99, [], default));

        Assert.Equal("CMN_MSG_MESSAGE_FROM_OTHER_CONVERSATION", exception.ErrorType);
    }

    [Fact]
    public async Task Edit_OthersMessage_IsForbidden()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        GivenMessage(new Message { Id = 5, ConversationId = 10, AuthorId = 2, Type = MessageType.Text, Text = "x" });

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Edit(5, "y", default));

        Assert.Equal("CMN_MSG_CANNOT_CHANGE_OTHERS_MESSAGE", exception.ErrorType);
    }

    [Fact]
    public async Task Edit_ASystemMessage_IsNotAllowed()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).Build());
        GivenMessage(new Message { Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.System, Text = "x" });

        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Edit(5, "y", default));

        Assert.Equal("CMN_MSG_CANNOT_CHANGE_MESSAGE", exception.ErrorType);
    }

    [Fact]
    public async Task Edit_MarksTheMessageEdited()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        var message = GivenMessage(new Message { Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.Text, Text = "Помилка" });

        await Command(() => MessageService.Edit(5, " Виправлено ", default));

        Assert.Equal("Виправлено", message.Text);
        Assert.NotNull(message.EditedAt);
        Publisher.Verify(x => x.MessageUpdated(Ids(Me, 2), It.Is<MessageVm>(m => m.Text == "Виправлено"), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Edit_WithTheSameText_ChangesNothing()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        var message = GivenMessage(new Message { Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.Text, Text = "Так" });

        await Command(() => MessageService.Edit(5, "Так", default));

        Assert.Null(message.EditedAt);
        Assert.Empty(Publisher.Invocations);
    }

    [Fact]
    public async Task Delete_KeepsTheMessageWithoutItsText()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        var message = GivenMessage(new Message { Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.Text, Text = "Секрет" });

        await Command(() => MessageService.Delete(5, default));

        Assert.True(message.IsDeleted);
        Assert.Null(message.Text);
        Publisher.Verify(x => x.MessageUpdated(Ids(Me, 2), It.Is<MessageVm>(m => m.IsDeleted && m.Text == null),
            It.IsAny<CancellationToken>()), Times.Once);

        var again = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Delete(5, default));
        Assert.Equal("CMN_MSG_CANNOT_CHANGE_MESSAGE", again.ErrorType);
    }

    [Fact]
    public async Task GetPage_LimitsThePageSize()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        Messages.Setup(x => x.GetPage(10, null, It.IsAny<int>(), default)).ReturnsAsync([]);

        await MessageService.GetPage(10, null, 1000, default);

        Messages.Verify(x => x.GetPage(10, null, 100, default), Times.Once);
    }

    private Message GivenMessage(Message message)
    {
        Messages.Setup(x => x.GetForUpdate(message.Id, It.IsAny<CancellationToken>())).ReturnsAsync(message);
        Messages.Setup(x => x.GetById(message.Id, It.IsAny<CancellationToken>())).ReturnsAsync(message);
        return message;
    }
}
