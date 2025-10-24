using Generators.Pdf.Application.Common.Dto;
using Generators.Pdf.Application.Common.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Repositories;

public class PuppeteerSharpRepository : IPuppeteerSharpRepository
{
    public async Task<Stream> GenerateFromHtml(GeneratePdfDto dto)
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