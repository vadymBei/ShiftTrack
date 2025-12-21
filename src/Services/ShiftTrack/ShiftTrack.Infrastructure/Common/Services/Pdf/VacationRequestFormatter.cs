using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Vacations.Queries.DownloadVacationRequestPdf;

namespace ShiftTrack.Infrastructure.Common.Services.Pdf;

public class VacationRequestFormatter(
    IHostEnvironment hostEnvironment) : IPdfFormatter<VacationRequestData>
{
    public string Format(VacationRequestData data)
    {
        var path = Path.Combine(
            hostEnvironment.ContentRootPath,
            "wwwroot", 
            "static", 
            "templates", 
            "booking",
            "vacations",
            "VacationRequest.html");

        var htmlDocument = new HtmlDocument()
        {
            OptionReadEncoding = false
        };
  
        htmlDocument.Load(path);
        
        return htmlDocument.DocumentNode.OuterHtml;
    }
}