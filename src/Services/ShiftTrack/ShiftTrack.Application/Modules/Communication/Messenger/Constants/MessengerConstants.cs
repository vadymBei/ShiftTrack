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

    public const long MaxAttachmentSize = 20 * 1024 * 1024;
    public const int MaxAttachmentsPerMessage = 10;

    /// <summary>More mentions in one message are kept as text but nobody else is notified.</summary>
    public const int MaxMentionsPerMessage = 50;

    public const int MentionNotificationLength = 200;

    /// <summary>Uploads not sent in a message are removed after this time.</summary>
    public static readonly TimeSpan UnsentAttachmentLifetime = TimeSpan.FromDays(1);

    /// <summary>
    /// Documents, images and archives people send at work, with the type the file is served with.
    /// No executables and no SVG/HTML: they could run in the browser.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> AttachmentTypes = new Dictionary<string, string>(
        StringComparer.OrdinalIgnoreCase)
    {
        [".pdf"] = "application/pdf",
        [".doc"] = "application/msword",
        [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".xls"] = "application/vnd.ms-excel",
        [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [".ppt"] = "application/vnd.ms-powerpoint",
        [".pptx"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        [".odt"] = "application/vnd.oasis.opendocument.text",
        [".ods"] = "application/vnd.oasis.opendocument.spreadsheet",
        [".rtf"] = "application/rtf",
        [".txt"] = "text/plain",
        [".csv"] = "text/csv",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".gif"] = "image/gif",
        [".webp"] = "image/webp",
        [".heic"] = "image/heic",
        [".zip"] = "application/zip",
        [".rar"] = "application/vnd.rar",
        [".7z"] = "application/x-7z-compressed"
    };

    /// <summary>Types the browser may show in a tab; the rest is always downloaded.</summary>
    public static readonly IReadOnlySet<string> InlineAttachmentTypes = new HashSet<string>
    {
        "application/pdf", "image/jpeg", "image/png", "image/gif", "image/webp"
    };
}
