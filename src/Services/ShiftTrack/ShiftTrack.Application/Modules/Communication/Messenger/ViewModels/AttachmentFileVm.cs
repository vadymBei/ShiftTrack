namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

/// <summary>The file to send in the response.</summary>
public class AttachmentFileVm
{
    public Stream Content { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }

    /// <summary>Shown in the browser (images, PDF) unless a download is asked for.</summary>
    public bool Inline { get; set; }
}
