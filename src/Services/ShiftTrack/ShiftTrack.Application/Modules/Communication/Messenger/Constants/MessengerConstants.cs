namespace ShiftTrack.Application.Modules.Communication.Messenger.Constants;

public static class MessengerConstants
{
    public const int MaxMessageLength = 4000;
    public const int MaxTitleLength = 100;
    public const int MaxGroupMembers = 200;

    public const int DefaultMessagesPage = 50;
    public const int MaxMessagesPage = 100;
    public const int MaxConversations = 200;
    public const int MaxContacts = 20;

    /// <summary>At most this many messages per employee in <see cref="RateLimitWindow"/>.</summary>
    public const int RateLimitMessages = 20;
    public static readonly TimeSpan RateLimitWindow = TimeSpan.FromSeconds(10);

    public const int ReplyPreviewLength = 200;
}
