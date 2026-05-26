using Generators.Pdf.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Generators.Pdf.Application.UseCases.Queries.GeneratePdfFromHtml;

public class GeneratePdfFromHtmlQueryHandler(
    IPdfGeneratorService pdfGeneratorService) : IRequestHandler<GeneratePdfFromHtmlQuery, Stream>
{
    public Task<Stream> Handle(GeneratePdfFromHtmlQuery request, CancellationToken cancellationToken = default)
    {
        return pdfGeneratorService.GenerateFromHtml(request.Dto, cancellationToken);
    }
}