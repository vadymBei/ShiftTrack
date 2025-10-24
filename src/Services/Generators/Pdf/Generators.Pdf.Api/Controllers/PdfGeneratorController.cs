using Generators.Pdf.Application.Common.Dto;
using Generators.Pdf.Application.Queries.GeneratePdfFromHtml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace Generators.Pdf.Api.Controllers;

[Authorize]
[Route("api/generators/pdf")]
public class PdfGeneratorController : ApiController
{
    [HttpPost("generate/from-html")]
    public async Task<Stream> GenerateFromHtml([FromBody] GeneratePdfDto dto)
    {
        var pdfStream = await Mediator.Invoke(new GeneratePdfFromHtmlQuery(dto));

        return pdfStream;
    }

    [HttpPost("generate/from-html/file")]
    public async Task<FileResult> GenerateFromHtmlAsFile([FromBody] GeneratePdfDto dto)
    {
        var pdfStream = await Mediator.Invoke(new GeneratePdfFromHtmlQuery(dto));

        return File(pdfStream, "application/pdf", "document.pdf");
    }
}