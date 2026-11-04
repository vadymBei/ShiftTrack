using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Events;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class MentionTests : MessengerFixture
{
    [Fact]
    public void Tokens_AreParsedInOrderWithoutRepeats()
    {
        Assert.Equal([12L, 3L], MentionTokens.Parse("<@12> і <@3>, ще раз <@12>; не токен: <@x> @12"));
        Assert.Empty(MentionTokens.Parse(null));
    }

    [Fact]
    public void PlainText_ShowsNames()
    {
        var text = MentionTokens.ToPlainText("<@12>, глянь, будь ласка. <@99>", id => id == 12 ? "Шевчук Ірина" : null);

        Assert.Equal("@Шевчук Ірина, глянь, будь ласка. @працівник", text);
    }

    [Fact]
    public async Task Send_InAGroup_MentionsMembersOnly_AndNotifiesThroughTheOutbox()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).WithMember(3).Build());

        var message = await Command(() => MessageService.Send(10, $"<@2> <@3> <@{Me}> <@77>, хто завтра?", null, null, [], default));

        Assert.Equal([2L, 3L], message.Mentions.Select(x => x.EmployeeId));
        Outbox.Verify(x => x.Enqueue(It.Is<MessageMentionsAddedEvent>(e => e.MessageId == message.Id
            && e.EmployeeIds.SequenceEqual(new long[] { 2, 3 })), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_InADirectChat_HasNoMentions()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());

        var message = await Command(() => MessageService.Send(10, "<@2> привіт", null, null, [], default));

        Assert.Empty(message.Mentions);
        Outbox.Verify(x => x.Enqueue(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Edit_NotifiesOnlyTheNewlyMentioned()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).WithMember(3).Build());
        var message = new Message
        {
            Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.Text, Text = "<@2> привіт",
            Mentions = [new MessageMention { MessageId = 5, EmployeeId = 2 }]
        };
        Messages.Setup(x => x.GetForUpdate(5, It.IsAny<CancellationToken>())).ReturnsAsync(message);
        Messages.Setup(x => x.GetById(5, It.IsAny<CancellationToken>())).ReturnsAsync(message);

        await Command(() => MessageService.Edit(5, "<@3> привіт", default));

        Assert.Equal([3L], message.Mentions.Select(x => x.EmployeeId));
        Outbox.Verify(x => x.Enqueue(It.Is<MessageMentionsAddedEvent>(e => e.EmployeeIds.SequenceEqual(new long[] { 3 })),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Edit_KeepingTheSameMentions_NotifiesNobody()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).Build());
        var message = new Message
        {
            Id = 5, ConversationId = 10, AuthorId = Me, Type = MessageType.Text, Text = "<@2> привіт",
            Mentions = [new MessageMention { MessageId = 5, EmployeeId = 2 }]
        };
        Messages.Setup(x => x.GetForUpdate(5, It.IsAny<CancellationToken>())).ReturnsAsync(message);
        Messages.Setup(x => x.GetById(5, It.IsAny<CancellationToken>())).ReturnsAsync(message);

        await Command(() => MessageService.Edit(5, "<@2> привіт усім", default));

        Outbox.Verify(x => x.Enqueue(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Mute_TellsOnlyTheUsersOtherTabs()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).Build());
        Conversations.Setup(x => x.SetMuted(10, Me, true, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        await Command(() => ConversationService.SetMuted(10, true, default));

        Publisher.Verify(x => x.ConversationChanged(Ids(Me), 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}
