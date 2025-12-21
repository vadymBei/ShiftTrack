namespace ShiftTrack.Application.Common.Dtos;

public class PdfTemplateDto
{
    public string Html { get; set; }
    public string MarginTop { get; set; } = "0px";
    public string MarginBottom { get; set; } = "0px";
    public string MarginRight { get; set; } = "0px";
    public string MarginLeft { get; set; } = "0px";
}