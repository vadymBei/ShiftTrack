namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

/// <summary>The message a reply refers to.</summary>
public class MessagePreviewVm
{
    public long Id { get; set; }
    public long? AuthorId { get; set; }
    public string AuthorFullName { get; set; }
    public string Text { get; set; }
    public bool IsDeleted { get; set; }
}
