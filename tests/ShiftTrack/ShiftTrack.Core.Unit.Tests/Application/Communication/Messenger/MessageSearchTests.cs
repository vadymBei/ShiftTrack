using Moq;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Core.Unit.Tests.Builders;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class MessageSearchTests : MessengerFixture
{
    private readonly List<MessageSearchCandidate> _candidates = [];

    public MessageSearchTests()
    {
        Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());
        Messages
            .Setup(x => x.Search(10, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _candidates);
    }

    [Fact]
    public async Task Search_EscapesTheLikePattern()
    {
        await MessageService.Search(10, "  50%_a\\b ", default);

        Messages.Verify(x => x.Search(10, "%50\\%\\_a\\\\b%", 200, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Search_MatchesTheTextAsShown_NotTheMentionTokens()
    {
        _candidates.Add(Candidate(3, "<@22> на касу", mentions: new() { [22] = "Олена Коваль" }));
        _candidates.Add(Candidate(2, "Олена, привіт"));
        _candidates.Add(Candidate(1, "Звіт", files: ["Олена-графік.xlsx"]));

        var byName = await MessageService.Search(10, "олена", default);
        var byToken = await MessageService.Search(10, "<@22", default);

        Assert.Equal([3, 2, 1], byName.MessageIds);
        Assert.False(byName.HasMore);
        Assert.Empty(byToken.MessageIds);
    }

    [Fact]
    public async Task Search_GivesTheNewestHundred_AndSaysThereIsMore()
    {
        for (var id = 150; id >= 1; id--)
            _candidates.Add(Candidate(id, "поставка " + id));

        var result = await MessageService.Search(10, "поставка", default);

        Assert.Equal(100, result.MessageIds.Count);
        Assert.Equal(150, result.MessageIds[0]);
        Assert.True(result.HasMore);
    }

    [Theory]
    [InlineData(" п ", "CMN_MSG_SEARCH_TOO_SHORT")]
    [InlineData(null, "CMN_MSG_SEARCH_TOO_SHORT")]
    public async Task Search_OfTooLittle_IsRejected(string query, string error)
    {
        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Search(10, query, default));

        Assert.Equal(error, exception.ErrorType);
    }

    [Fact]
    public async Task Search_InAConversationOfOthers_IsForbidden()
    {
        var exception = await Assert.ThrowsAsync<MessengerException>(() => MessageService.Search(99, "поставка", default));

        Assert.Equal("CMN_MSG_NOT_A_MEMBER", exception.ErrorType);
    }

    [Fact]
    public async Task PageUntil_ReachesTheMessage_WithAPageBeforeIt()
    {
        Messages.Setup(x => x.GetPageUntil(10, 80, 12, It.IsAny<int>(), It.IsAny<int>(), default)).ReturnsAsync([]);

        await MessageService.GetPageUntil(10, 80, 12, 1000, default);

        Messages.Verify(x => x.GetPageUntil(10, 80, 12, 100, 1000, default), Times.Once);
    }

    private static MessageSearchCandidate Candidate(long id, string text, List<string> files = null,
        Dictionary<long, string> mentions = null) =>
        new(id, text, files ?? [], mentions ?? new Dictionary<long, string>());
}
