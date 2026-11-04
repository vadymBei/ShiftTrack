namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

public class AttachmentVm
{
    public long Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public bool IsImage { get; set; }
}
