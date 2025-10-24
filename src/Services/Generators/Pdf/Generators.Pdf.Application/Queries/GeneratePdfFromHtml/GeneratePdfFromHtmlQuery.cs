using Generators.Pdf.Application.Common.Dto;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Generators.Pdf.Application.Queries.GeneratePdfFromHtml;

public record GeneratePdfFromHtmlQuery(
    GeneratePdfDto Dto) : IRequest<Stream>;