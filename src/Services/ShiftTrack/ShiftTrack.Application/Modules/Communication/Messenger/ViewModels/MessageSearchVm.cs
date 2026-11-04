namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

public class MessageSearchVm
{
    /// <summary>The newest first.</summary>
    public List<long> MessageIds { get; set; } = [];

    /// <summary>There are older matches than these.</summary>
    public bool HasMore { get; set; }
}
