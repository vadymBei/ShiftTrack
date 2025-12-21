using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Common.Dtos;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Repositories;

public class PdfRepository(
    IClient client,
    ILogger<PdfRepository> logger) : IPdfRepository
{
    public async Task<Stream> GenerateFromHtml(PdfTemplateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var stream = await client
                .Path("pdf-generator-api/pdf-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Post<Stream>("generate/from-html", cancellationToken);
            
            return stream;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while generating PDF");
            throw;
        }
    }
}