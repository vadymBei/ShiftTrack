namespace ShiftTrack.Application.Modules.Communication.Messenger.Constants;

public static class MessengerExceptionsLocalization
{
    public const string NOT_A_MEMBER = "You are not a member of this conversation.";
    public const string SEARCH_TOO_SHORT = "Type at least 2 characters to search.";
    public const string SEARCH_TOO_LONG = "The search text is too long.";
    public const string CANNOT_CHAT_WITH_YOURSELF = "You cannot start a conversation with yourself.";
    public const string NOT_A_GROUP = "This action is available only in group conversations.";
    public const string ONLY_OWNER = "Only the owner of the group can do this.";
    public const string CANNOT_REMOVE_YOURSELF = "Leave the group instead of removing yourself.";
    public const string MEMBER_NOT_FOUND = "The employee is not a member of this group.";
    public const string EMPLOYEES_NOT_FOUND = "Some of the employees are not found.";
    public const string GROUP_NEEDS_MEMBERS = "A group needs at least one more member.";
    public const string TOO_MANY_MEMBERS = "The group has too many members.";
    public const string RECIPIENT_UNAVAILABLE = "The employee is no longer available.";
    public const string CANNOT_CHANGE_OTHERS_MESSAGE = "You can only change your own messages.";
    public const string CANNOT_CHANGE_MESSAGE = "This message cannot be changed.";
    public const string MESSAGE_FROM_OTHER_CONVERSATION = "The message belongs to another conversation.";
    public const string TOO_MANY_MESSAGES = "You are sending messages too fast.";
    public const string ATTACHMENT_EMPTY = "The file is empty.";
    public const string ATTACHMENT_TOO_LARGE = "The file is larger than 20 MB.";
    public const string ATTACHMENT_TYPE_NOT_ALLOWED = "Files of this type can't be sent.";
    public const string TOO_MANY_ATTACHMENTS = "A message can have at most 10 files.";
    public const string ATTACHMENTS_NOT_FOUND = "Some of the files are not found or are already sent.";
    public const string ATTACHMENT_NOT_FOUND = "The file is not found.";
    public const string ATTACHMENT_STORAGE_UNAVAILABLE = "The server can't save files right now.";
}
