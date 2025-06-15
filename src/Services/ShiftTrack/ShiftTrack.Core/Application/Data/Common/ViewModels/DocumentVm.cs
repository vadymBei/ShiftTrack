using System.Security.AccessControl;

namespace ShiftTrack.Core.Application.Data.Common.ViewModels;

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
            _ => "application/octet-stream"
        };

    } 
}