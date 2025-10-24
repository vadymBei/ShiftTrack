namespace Generators.Pdf.Application.Common.Dto;
    
public class GeneratePdfDto
{
    public string Html { get; set; }
    public string MarginTop { get; set; } = "0px";
    public string MarginBottom { get; set; } = "0px";
    public string MarginLeft { get; set; } = "0px";
    public string MarginRight { get; set; } = "0px";
}