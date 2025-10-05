using Generators.Pdf.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace Generators.Pdf.Api.Controllers;

[Authorize]
[Route("api/generators/pdf")]
public class PdfGeneratorController : ApiController
{
    [HttpPost("generate/from-html")]
    public async Task<Stream> GenerateFromHtml([FromBody] GeneratePdfRequestDto dto)
    {
        var pdfStream = await GeneratePdfFromHtml(dto);
        
        return pdfStream;
    }

    [HttpPost("generate/from-html/file")]
    public async Task<FileResult> GenerateFromHtmlAsFile([FromBody] GeneratePdfRequestDto dto)
    {
        var pdfStream = await GeneratePdfFromHtml(dto);
        
        return File(pdfStream, "application/pdf", "document.pdf");
    }

    private static async Task<Stream> GeneratePdfFromHtml(GeneratePdfRequestDto dto)
    {
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            ExecutablePath = "/usr/bin/chromium",
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
        };

        await using var browser = await Puppeteer.LaunchAsync(launchOptions);
        
        await using var page = await browser.NewPageAsync();
        
        await page.SetContentAsync(dto.Html);

        var pdfStream = await page.PdfStreamAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            MarginOptions = new MarginOptions
            {
                Top = dto.MarginTop,
                Bottom = dto.MarginBottom,
                Right = dto.MarginRight,
                Left = dto.MarginLeft
            }
        });

        return pdfStream;
    }
}