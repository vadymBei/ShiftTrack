using ShiftTrack.Application.Common.Constants;

namespace ShiftTrack.Application.Common.ViewModels;

public class DocumentVm
{
    public string Path { get; set; }
    public Stream StreamContent { get; set; }
    public byte[] Content { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public string MimeType => Extension switch
    {
        FileExtensions.Jpg or ".jpeg" => "image/jpeg",
        FileExtensions.Png => "image/png",
        FileExtensions.Pdf => "application/pdf",
        FileExtensions.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        _ => "application/octet-stream"
    };
}