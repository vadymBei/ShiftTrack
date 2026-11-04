using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class ConversationServiceTests : MessengerFixture
{
    [Fact]
    public async Task ChatWithYourself_IsNotAllowed()
    {
        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.OpenDirect(Me, default));

        Assert.Equal("CMN_MSG_CANNOT_CHAT_WITH_YOURSELF", exception.ErrorType);
    }

    [Fact]
    public async Task OpenDirect_SendsNothingUntilTheFirstMessage()
    {
        Conversations.Setup(x => x.GetOrCreateDirect(Me, 2, default)).ReturnsAsync(10);

        var summary = await Command(() => ConversationService.OpenDirect(2, default));

        Assert.Equal(10, summary.Conversation.Id);
        Assert.Empty(Publisher.Invocations);
    }

    [Fact]
    public async Task Group_NeedsSomeoneBesidesTheCreator()
    {
        var exception = await Assert.ThrowsAsync<MessengerException>(() =>
            ConversationService.CreateGroup("Склад", [Me], default));

        Assert.Equal("CMN_MSG_GROUP_NEEDS_MEMBERS", exception.ErrorType);
    }

    [Fact]
    public async Task Group_WithUnknownEmployees_IsNotCreated()
    {
        EmployeesExist(2);

        var exception = await Assert.ThrowsAsync<MessengerException>(() =>
            ConversationService.CreateGroup("Склад", [2, 3], default));

        Assert.Equal("CMN_MSG_EMPLOYEES_NOT_FOUND", exception.ErrorType);
        Conversations.Verify(x => x.Add(It.IsAny<Conversation>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateGroup_MakesTheCreatorOwner_AndTellsEveryMember()
    {
        EmployeesExist(2, 3);
        Conversation created = null;
        Conversations
            .Setup(x => x.Add(It.IsAny<Conversation>(), It.IsAny<CancellationToken>()))
            .Callback<Conversation, CancellationToken>((conversation, _) => (created = conversation).Id = 10)
            .Returns(Task.CompletedTask);

        await Command(() => ConversationService.CreateGroup("  Склад ", [2, 3, 3, Me], default));

        Assert.Equal("Склад", created.Title);
        Assert.Equal(ConversationType.Group, created.Type);
        Assert.Equal([Me, 2, 3], created.Members.Select(x => x.EmployeeId).OrderBy(x => x));
        Assert.True(created.IsOwner(Me));
        Messages.Verify(x => x.Add(It.Is<Message>(m => m.Type == MessageType.System && m.Text.Contains("створила групу «Склад»")),
            It.IsAny<CancellationToken>()), Times.Once);
        Publisher.Verify(x => x.ConversationChanged(Ids(Me, 2, 3), 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Rename_IsForTheOwnerOnly()
    {
        Existing(ConversationBuilder.Group().WithOwner(2).WithMember(Me).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.Rename(10, "Нова", default));

        Assert.Equal("CMN_MSG_ONLY_OWNER", exception.ErrorType);
    }

    [Fact]
    public async Task Rename_ADirectChat_IsNotAllowed()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.Rename(10, "Нова", default));

        Assert.Equal("CMN_MSG_NOT_A_GROUP", exception.ErrorType);
    }

    [Fact]
    public async Task WhenTheOwnerIsGone_TheEarliestMemberManagesTheGroup()
    {
        // The owner is a deleted employee, so the membership isn't loaded.
        var group = Existing(ConversationBuilder.Group().WithMember(Me).WithMember(3).Build());

        await Command(() => ConversationService.Rename(10, "Нова", default));

        Assert.Equal("Нова", group.Title);
    }

    [Fact]
    public async Task OutsiderCannotDoAnything()
    {
        Existing(ConversationBuilder.Group().WithOwner(2).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.AddMembers(10, [Me], default));

        Assert.Equal("CMN_MSG_NOT_A_MEMBER", exception.ErrorType);
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, exception.Code);
    }

    [Fact]
    public async Task AddMembers_NewMemberDoesNotGetTheHistoryAsUnread()
    {
        var group = Existing(ConversationBuilder.Group().WithOwner(2).WithMember(Me).Build());
        group.LastMessageId = 55;
        EmployeesExist(4);

        await Command(() => ConversationService.AddMembers(10, [4, 2], default));

        var added = group.FindMember(4);
        Assert.Equal(ConversationMemberRole.Member, added.Role);
        Assert.Equal(55, added.LastReadMessageId);
        Assert.Equal(3, group.Members.Count);
        Publisher.Verify(x => x.ConversationChanged(Ids(Me, 2, 4), 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddMembers_WithEveryoneAlreadyThere_ChangesNothing()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).Build());

        await Command(() => ConversationService.AddMembers(10, [2], default));

        Conversations.Verify(x => x.Save(It.IsAny<CancellationToken>()), Times.Never);
        Assert.Empty(Publisher.Invocations);
    }

    [Fact]
    public async Task RemoveMember_AlsoTellsTheRemovedOne()
    {
        var group = Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).WithMember(3).Build());

        await Command(() => ConversationService.RemoveMember(10, 3, default));

        Assert.Null(group.FindMember(3));
        Publisher.Verify(x => x.MessageCreated(Ids(Me, 2), It.IsAny<ShiftTrack.Application.Modules.Communication.Messenger.ViewModels.MessageVm>(),
            It.IsAny<CancellationToken>()), Times.Once);
        Publisher.Verify(x => x.ConversationChanged(Ids(Me, 2, 3), 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveYourself_IsNotAllowed()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.RemoveMember(10, Me, default));

        Assert.Equal("CMN_MSG_CANNOT_REMOVE_YOURSELF", exception.ErrorType);
    }

    [Fact]
    public async Task OwnerLeaves_TheEarliestMemberBecomesOwner()
    {
        var group = Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(3).WithMember(2).Build());

        await Command(() => ConversationService.Leave(10, default));

        Assert.Null(group.FindMember(Me));
        Assert.True(group.IsOwner(3));
        Assert.False(group.IsOwner(2));
        Messages.Verify(x => x.Add(It.Is<Message>(m => m.Text.Contains("тепер власник групи")), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LastMemberLeaves_TheGroupIsDeleted()
    {
        var group = Existing(ConversationBuilder.Group().WithOwner(Me).Build());

        await Command(() => ConversationService.Leave(10, default));

        Conversations.Verify(x => x.Remove(group, It.IsAny<CancellationToken>()), Times.Once);
        Messages.Verify(x => x.Add(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MarkRead_TellsOthersOnlyWhenThePositionMoves()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        Messages.Setup(x => x.Exists(10, 30, default)).ReturnsAsync(true);
        Conversations.SetupSequence(x => x.MarkRead(10, Me, 30, default)).ReturnsAsync(true).ReturnsAsync(false);

        await Command(() => ConversationService.MarkRead(10, 30, default));
        await Command(() => ConversationService.MarkRead(10, 30, default));

        Publisher.Verify(x => x.ConversationRead(Ids(Me, 2),
            It.Is<ShiftTrack.Application.Modules.Communication.Messenger.ViewModels.ConversationReadVm>(r => r.LastReadMessageId == 30 && r.EmployeeId == Me),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MarkRead_AMessageOfAnotherChat_IsRejected()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => ConversationService.MarkRead(10, 99, default));

        Assert.Equal("CMN_MSG_MESSAGE_FROM_OTHER_CONVERSATION", exception.ErrorType);
    }

    [Fact]
    public async Task Typing_GoesToOthersOnly()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).WithMember(2).WithMember(3).Build());

        await Command(() => ConversationService.Typing(10, default));

        Publisher.Verify(x => x.Typing(Ids(2, 3), It.IsAny<ShiftTrack.Application.Modules.Communication.Messenger.ViewModels.TypingVm>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
