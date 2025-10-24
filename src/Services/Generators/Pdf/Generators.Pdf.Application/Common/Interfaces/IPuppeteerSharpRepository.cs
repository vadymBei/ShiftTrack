using Generators.Pdf.Application.Common.Dto;

namespace Generators.Pdf.Application.Common.Interfaces;

public interface IPuppeteerSharpRepository
{
    Task<Stream> GenerateFromHtml(GeneratePdfDto dto);
}