using Generators.Pdf.Application.Common.Dto;

namespace Generators.Pdf.Application.Common.Interfaces;

public interface IPdfGeneratorService
{
    Task<Stream> GenerateFromHtml(GeneratePdfDto dto);
}