using Generators.Pdf.Application.Common.Dto;
using Generators.Pdf.Application.Common.Interfaces;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Repositories;

public class PuppeteerSharpRepository(
    IBrowserManager browserManager) : IPuppeteerSharpRepository
{
    private readonly SemaphoreSlim _semaphore = new(5);

    public async Task<Stream> GenerateFromHtml(GeneratePdfDto dto, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var browser = await browserManager.GetBrowserAsync();

            await using var page = await browser.NewPageAsync();
            
            page.DefaultTimeout = 30000;

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
        finally
        {
            _semaphore.Release();
        }
    }
}