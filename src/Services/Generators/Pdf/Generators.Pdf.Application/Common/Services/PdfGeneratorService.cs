using Generators.Pdf.Application.Common.Dto;
using Generators.Pdf.Application.Common.Interfaces;

namespace Generators.Pdf.Application.Common.Services;

public class PdfGeneratorService(
    IPuppeteerSharpRepository puppeteerSharpRepository) : IPdfGeneratorService
{
    public Task<Stream> GenerateFromHtml(GeneratePdfDto dto)
        => puppeteerSharpRepository.GenerateFromHtml(dto);
}