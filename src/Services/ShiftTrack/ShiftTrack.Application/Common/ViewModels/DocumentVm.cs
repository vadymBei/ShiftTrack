namespace ShiftTrack.Application.Common.ViewModels;

public class DocumentVm
{
    public string Path { get; set; }
    public byte[] Content { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public string GetMimeType()
    {
        return Extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream"
        };

    } 
}