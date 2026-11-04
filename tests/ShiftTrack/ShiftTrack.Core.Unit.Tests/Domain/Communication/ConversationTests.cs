using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Core.Unit.Tests.Domain.Communication;

public class ConversationTests
{
    [Fact]
    public void DirectKey_DoesNotDependOnWhoStartsTheChat()
    {
        Assert.Equal("3:17", Conversation.DirectKeyFor(3, 17));
        Assert.Equal("3:17", Conversation.DirectKeyFor(17, 3));
    }

    [Fact]
    public void IsOwner_OnlyForTheOwnerMember()
    {
        var group = ConversationBuilder.Group().WithOwner(1).WithMember(2).Build();

        Assert.True(group.IsOwner(1));
        Assert.False(group.IsOwner(2));
        Assert.False(group.IsOwner(3));
        Assert.Null(group.FindMember(3));
    }

    [Fact]
    public void Message_IsDeletedWhenItHasDeletedAt()
    {
        Assert.False(new Message().IsDeleted);
        Assert.True(new Message { DeletedAt = DateTime.UtcNow }.IsDeleted);
    }
}
