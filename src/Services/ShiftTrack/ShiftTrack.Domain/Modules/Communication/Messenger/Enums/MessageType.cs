namespace ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

public enum MessageType
{
    None = 0,
    Text = 1,

    /// <summary>A change in the conversation: created, renamed, members added or removed.</summary>
    System = 2
}
