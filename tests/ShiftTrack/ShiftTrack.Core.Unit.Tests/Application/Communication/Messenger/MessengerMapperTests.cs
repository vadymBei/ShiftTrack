using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class MessengerMapperTests
{
    [Fact]
    public void DirectChat_IsNamedAfterTheOtherMember()
    {
        var chat = ConversationBuilder.Direct().WithMember(1).WithMember(2).Build();

        var vm = new ConversationSummary { Conversation = chat }.ToVm(currentEmployeeId: 1);

        Assert.Equal("Працівник2 Тест ", vm.Title);
        Assert.Equal(2, vm.Interlocutor.EmployeeId);
        Assert.True(vm.CanWrite);
    }

    [Fact]
    public void DirectChat_WithAnUnavailableEmployee_CannotBeWrittenTo()
    {
        // A deleted employee's membership is filtered out when the conversation is loaded.
        var chat = ConversationBuilder.Direct().WithMember(1).Build();

        var vm = new ConversationSummary { Conversation = chat }.ToVm(currentEmployeeId: 1);

        Assert.Null(vm.Interlocutor);
        Assert.False(vm.CanWrite);
    }

    [Fact]
    public void Group_ShowsTheOwnerFirstAndMyRole()
    {
        var group = ConversationBuilder.Group("Склад")
            .WithMember(2)
            .WithOwner(1)
            .Build();

        var vm = new ConversationSummary { Conversation = group, UnreadCount = 4 }.ToVm(currentEmployeeId: 2);

        Assert.Equal("Склад", vm.Title);
        Assert.Equal(1, vm.Members[0].EmployeeId);
        Assert.Equal(ConversationMemberRole.Member, vm.MyRole);
        Assert.Equal(4, vm.UnreadCount);
        Assert.Null(vm.Interlocutor);
    }

    [Fact]
    public void ReadByOthers_IsTheFurthestReadPositionOfOtherMembers()
    {
        var group = ConversationBuilder.Group()
            .WithOwner(1)
            .WithMember(2, lastReadMessageId: 7)
            .WithMember(3, lastReadMessageId: 12)
            .Build();
        group.FindMember(1).LastReadMessageId = 20;

        var vm = new ConversationSummary { Conversation = group }.ToVm(currentEmployeeId: 1);

        Assert.Equal(12, vm.ReadByOthersUpToMessageId);
    }

    [Fact]
    public void LastActivity_IsTheLastMessageOrTheCreation()
    {
        var group = ConversationBuilder.Group().WithOwner(1).Build();

        Assert.Equal(group.CreatedAt, new ConversationSummary { Conversation = group }.ToVm(1).LastActivityAt);

        group.LastMessageAt = group.CreatedAt.AddHours(2);
        Assert.Equal(group.LastMessageAt, new ConversationSummary { Conversation = group }.ToVm(1).LastActivityAt);
    }

    [Fact]
    public void DeletedMessage_HasNoText()
    {
        var vm = new Message { Id = 1, Text = "секрет", DeletedAt = DateTime.UtcNow }.ToVm();

        Assert.True(vm.IsDeleted);
        Assert.Null(vm.Text);
    }

    [Fact]
    public void Reply_ShowsAShortPreviewOfTheRepliedMessage()
    {
        var message = new Message
        {
            Id = 2,
            Text = "Згоден",
            ReplyToMessageId = 1,
            ReplyToMessage = new Message
            {
                Id = 1,
                AuthorId = 5,
                Author = ConversationBuilder.Employee(5),
                Text = new string('а', 300)
            }
        };

        var vm = message.ToVm();

        Assert.Equal(1, vm.ReplyTo.Id);
        Assert.Equal("Коваль Олена ", vm.ReplyTo.AuthorFullName);
        Assert.Equal(201, vm.ReplyTo.Text.Length);
        Assert.EndsWith("…", vm.ReplyTo.Text);
    }
}
