using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Vacations.Queries.DownloadVacationRequestPdf;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Infrastructure.Common.Services.Pdf;

public class VacationRequestFormatter(
    IHostEnvironment hostEnvironment) : IPdfFormatter<VacationRequestData>
{
    private readonly CultureInfo _culture = new("uk-UA");

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

        if (data.UnitDirector is not null)
        {
            var unitDirector = htmlDocument.GetElementbyId("unit_director");
            unitDirector.InnerHtml = data.UnitDirector.FullName;
        }

        var employeePosition = htmlDocument.GetElementbyId("employee_position");
        employeePosition.InnerHtml = data.Vacation.Employee.Position.Name.ToLower();

        var employee = htmlDocument.GetElementbyId("employee");
        employee.InnerHtml = data.Vacation.Employee.FullName;

        var vacationRequest = htmlDocument.GetElementbyId("vacation_request");
        vacationRequest.InnerHtml = GetVacationRequestText(data);

        var creationDate = htmlDocument.GetElementbyId("creation_date");
        creationDate.InnerHtml =
            $"«{data.Vacation.CreatedAt.Day}» {data.Vacation.CreatedAt.ToString("MMMM", _culture)} {data.Vacation.CreatedAt.Year} р.";

        var signatureEmployee = htmlDocument.GetElementbyId("signature_employee");
        signatureEmployee.InnerHtml = data.Vacation.Employee.FullName;

        
        return htmlDocument.DocumentNode.OuterHtml;
    }

    private string GetVacationRequestText(VacationRequestData data)
    {
        var vacationPeriod = $@" тривалістю <strong>{data.Vacation.DaysCount} календарних днів</strong> 
                   з <strong>«{data.Vacation.StartDate.Day}» {data.Vacation.StartDate.ToString("MMMM", _culture)} {data.Vacation.StartDate.Year} р.</strong> 
                   по <strong>«{data.Vacation.EndDate.Day}» {data.Vacation.EndDate.ToString("MMMM", _culture)} {data.Vacation.EndDate.Year} р.</strong> включно.";

        var vacationRequestText = "&nbsp";
        
        vacationRequestText += data.Vacation.Type switch
        {
            VacationType.YearMainVacation => "Прошу надати мені щорічну основну відпустку " + vacationPeriod,
            VacationType.VacationWithoutSalaryByFamily =>
                "Прошу надати відпустку без збереження заробітної плати за згодою сторін за сімейними обставинами" +
                vacationPeriod,
            VacationType.VacationWithoutSalaryByPregnancy =>
                "Прошу надати відпустку у зв`язку з вагітністю та пологами" + vacationPeriod,
            VacationType.VacationWithoutSalaryByChild3Years =>
                "Прошу надати відпустку для догляду за дитиною до досягнення нею трирічного віку" +
                vacationPeriod +
                " Копію свідоцтва про народження додаю.",
            VacationType.VacationWithoutSalaryByChild6Years =>
                "Прошу надати відпустку для догляду за дитиною до досягнення нею шестирічного віку" +
                vacationPeriod +
                " Копію свідоцтва про народження додаю.",
            _ => "Прошу надати мені щорічну основну відпустку" + vacationPeriod
        };
        
        return vacationRequestText;
    }
}