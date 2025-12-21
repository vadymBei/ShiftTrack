using ShiftTrack.Application.Common.Dtos;

namespace ShiftTrack.Application.Common.Interfaces;

public interface IPdfRepository
{
    Task<Stream> GenerateFromHtml(PdfTemplateDto dto, CancellationToken cancellationToken);
}