using ShiftTrack.Application.Common.Dtos;
using ShiftTrack.Application.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Services.Pdf;

public class PdfGenerator(
    IPdfRepository pdfRepository) : IPdfGenerator
{
    public async Task<Stream> Generate<T>(IPdfFormatter<T> formatter, T data, CancellationToken cancellationToken)
    {
        var outerHtml = formatter.Format(data);

        return await pdfRepository.GenerateFromHtml(
            new PdfTemplateDto()
            {
                Html = outerHtml,
                MarginBottom = "76px",
                MarginTop = "76px",
                MarginRight = "76px",
                MarginLeft = "113px"
            },
            cancellationToken);
    }
}