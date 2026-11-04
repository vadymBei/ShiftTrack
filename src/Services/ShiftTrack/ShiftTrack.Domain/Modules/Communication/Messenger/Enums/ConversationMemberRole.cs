namespace ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

public enum ConversationMemberRole
{
    None = 0,
    Member = 1,

    /// <summary>Renames the group and adds or removes members.</summary>
    Owner = 2
}
