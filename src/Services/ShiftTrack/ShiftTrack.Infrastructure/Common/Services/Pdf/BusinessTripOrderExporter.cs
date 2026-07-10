using System.Globalization;
using System.Text;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.Dtos;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.DownloadBusinessTripOrderPdf;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Infrastructure.Common.Services.Pdf;

public class BusinessTripOrderExporter(
    IPdfRepository pdfRepository,
    IHostEnvironment hostEnvironment) : IPdfExporter<BusinessTripOrderData>
{
    private readonly CultureInfo _culture = new("uk-UA");

    public async Task<Stream> Export(BusinessTripOrderData data, CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            hostEnvironment.ContentRootPath,
            "wwwroot",
            "static",
            "templates",
            "booking",
            "vacations",
            "BusinessTripOrder.html");

        var htmlDocument = new HtmlDocument()
        {
            OptionReadEncoding = false
        };

        htmlDocument.Load(path);

        var orderNumber = htmlDocument.GetElementbyId("order_number");
        orderNumber.InnerHtml = $"{data.BusinessTrip?.Author?.Department?.Unit?.Code}-{data.BusinessTrip?.Id}";

        var participants = data.BusinessTrip?.Participants.ToList();

        var employeesList = htmlDocument.GetElementbyId("employees_list");
        employeesList.InnerHtml = BuildEmployeesList(data, participants);

        var employeesCount = htmlDocument.GetElementbyId("employees_count");
        employeesCount.InnerHtml = participants.Count.ToString();

        var signaturesList = htmlDocument.GetElementbyId("signatures_list");
        signaturesList.InnerHtml = BuildSignaturesList(participants);

        return await pdfRepository.GenerateFromHtml(
            new PdfTemplateDto()
            {
                Html = htmlDocument.DocumentNode.OuterHtml,
                MarginBottom = "76px",
                MarginTop = "76px",
                MarginRight = "76px",
                MarginLeft = "113px"
            },
            cancellationToken);
    }

    private string BuildEmployeesList(BusinessTripOrderData data, List<Employee> participants)
    {
        var sb = new StringBuilder();
        var trip = data.BusinessTrip;

        var startDate = trip.StartDate.ToString("dd.MM.yyyy", _culture);
        var endDate = trip.EndDate.ToString("dd.MM.yyyy", _culture);
        var daysCount = (trip.EndDate.Date - trip.StartDate.Date).Days + 1;
        var destination = string.Join(", м. ", data.Locations.Select(x => x.Name));

        foreach (var employee in participants)
        {
            var positionName = employee.Position?.Name?.ToLower() ?? string.Empty;
            var departmentName = employee.Department?.Name != null ? $"'{employee.Department.Name}'" : string.Empty;
            var unitName = employee.Department?.Unit?.Name != null ? $"'{employee.Department?.Unit?.Name}'" : string.Empty;

            sb.Append(
                $"""
                 <li>Відрядити <strong>{employee.FullName}</strong>, 
                    {positionName}, регіону {unitName}, відділу {departmentName}, 
                    до м. {destination}, терміном на {daysCount} днів,
                    з {startDate} по {endDate} включно.</li>
                 """);
        }

        return sb.ToString();
    }

    private string BuildSignaturesList(List<Employee> participants)
    {
        var sb = new StringBuilder();

        foreach (var employee in participants)
        {
            var initials = GetInitials(employee);
            sb.Append(
                $@"<div class=""signature-item"">
                    <span class=""signature-line"">&nbsp;</span>
                    <span class=""signature-name"">{initials}</span>
                </div>");
        }

        return sb.ToString();
    }

    private static string GetInitials(Employee employee)
    {
        var surname = employee.Surname ?? string.Empty;

        var nameInitial = !string.IsNullOrEmpty(employee.Name)
            ? $"{employee.Name[0]}."
            : string.Empty;

        var patronymicInitial = !string.IsNullOrEmpty(employee.Patronymic)
            ? $"{employee.Patronymic[0]}."
            : string.Empty;

        return $"{surname} {nameInitial}{patronymicInitial}".Trim();
    }
}