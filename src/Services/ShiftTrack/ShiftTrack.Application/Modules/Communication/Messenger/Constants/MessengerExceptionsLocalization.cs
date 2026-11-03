namespace ShiftTrack.Application.Modules.Communication.Messenger.Constants;

public static class MessengerExceptionsLocalization
{
    public const string NOT_A_MEMBER = "You are not a member of this conversation.";
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
}
