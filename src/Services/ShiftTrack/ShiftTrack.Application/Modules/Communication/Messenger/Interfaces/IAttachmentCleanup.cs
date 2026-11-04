namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IAttachmentCleanup
{
    /// <summary>Removes uploads that were never sent. Returns how many.</summary>
    Task<int> RemoveUnsent(CancellationToken cancellationToken);
}
