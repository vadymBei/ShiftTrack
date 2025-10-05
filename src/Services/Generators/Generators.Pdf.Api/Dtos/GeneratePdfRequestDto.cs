namespace Generators.Pdf.Api.Dtos;

public class GeneratePdfRequestDto
{
    public string Html { get; set; }
    public string MarginTop { get; set; } = "0px";
    public string MarginBottom { get; set; } = "0px";
    public string MarginLeft { get; set; } = "0px";
    public string MarginRight { get; set; } = "0px";
}