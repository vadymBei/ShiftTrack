using System.Drawing;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Infrastructure.Common.Extensions;

namespace ShiftTrack.Infrastructure.Common.Services.Excel;

public class TimesheetPlanFormatter : IExcelFormatter<TimesheetExportData>
{
    #region Colors

    private const string HeaderColor = "#A5C2CA";
    private const string HeaderDayOffColor = "#EEB371";
    private const string TotalCellColor = "#FFE699";

    #endregion

    public byte[] Format(TimesheetExportData data)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Timesheet");

        var rowIndex = 1;

        ExcelRange range = null;

        #region Header

        range = worksheet.Cells[rowIndex, 1, rowIndex, 2];
        range.Merge = true;
        range.Value = "Період:";

        range = worksheet.Cells[rowIndex, 3, rowIndex, 5];
        range.Merge = true;
        range.Value =
            $"{data.Timesheet.StartDate.ToString(@"dd.MM.yyyy")} - {data.Timesheet.EndDate.ToString(@"dd.MM.yyyy")}";
        rowIndex++;

        range = worksheet.Cells[rowIndex, 1, rowIndex, 2];
        range.Merge = true;
        range.Value = "Регіон:";

        range = worksheet.Cells[rowIndex, 3, rowIndex, 5];
        range.Merge = true;
        range.Value = data.Timesheet.Department.Unit?.Name;
        rowIndex++;

        range = worksheet.Cells[rowIndex, 1, rowIndex, 2];
        range.Merge = true;
        range.Value = "Департамент:";

        range = worksheet.Cells[rowIndex, 3, rowIndex, 5];
        range.Merge = true;
        range.Value = data.Timesheet.Department.Name;
        rowIndex++;

        range = worksheet.Cells[1, 1, rowIndex, 1];
        range.Style.Font.Bold = true;

        rowIndex++;

        range = worksheet.Cells[rowIndex, 1, rowIndex, 5];
        range.Merge = true;
        range.Style.Font.Bold = true;
        range.Value = "Дата друку: " + DateTime.Now.ToString(@"dd.MM.yyyy HH:mm:ss");
        rowIndex++;

        #endregion

        #region Table

        var daysInPeriod = (data.Timesheet.EndDate - data.Timesheet.StartDate).Days + 1;

        worksheet.Cells[
                6,
                1,
                6 + 1 + data.Timesheet.EmployeeTimesheets.Count,
                daysInPeriod + 5]
            .SetRangeBorder();

        #region Table header

        var colIndex = 1;
        range = worksheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        range.Merge = true;
        range.Value = "№";
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(HeaderColor));
        colIndex++;

        range = worksheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        range.Merge = true;
        range.Value = "ПІБ";
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(HeaderColor));
        colIndex++;

        range = worksheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        range.Merge = true;
        range.Value = "Посада";
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(HeaderColor));
        colIndex++;

        var currentDate = data.Timesheet.StartDate;

        for (var day = 0; day < daysInPeriod; day++)
        {
            var dateForDay = currentDate.AddDays(day);

            var dayOfWeek = dateForDay
                .ToString("ddd", new CultureInfo("uk-UA"))
                .ToUpper();

            var isDayOff = dayOfWeek is "НД" or "СБ";

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = dateForDay.Day;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(
                ColorTranslator.FromHtml(isDayOff ? HeaderDayOffColor : HeaderColor));

            range = worksheet.Cells[rowIndex + 1, colIndex];
            range.Value = dayOfWeek;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(
                ColorTranslator.FromHtml(isDayOff ? HeaderDayOffColor : HeaderColor));

            colIndex++;
        }

        range = worksheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        range.Merge = true;
        range.Value = "Робочі години";
        range.Style.WrapText = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(TotalCellColor));
        colIndex++;

        range = worksheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        range.Merge = true;
        range.Value = "Робочі дні";
        range.Style.WrapText = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(TotalCellColor));

        range = worksheet.Cells[rowIndex, 1, rowIndex + 1, colIndex];
        range.Style.Font.Bold = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        rowIndex += 2;

        #endregion

        #region Table body

        var employeeIndex = 1;

        colIndex = 1;

        foreach (var employeeTimesheet in data.Timesheet.EmployeeTimesheets)
        {
            worksheet.Row(rowIndex).Height = 30;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Value = employeeIndex;
            colIndex++;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = employeeTimesheet.Employee.Surname + " " + employeeTimesheet.Employee.Name;
            colIndex++;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = employeeTimesheet.Employee.Position?.Name;
            colIndex++;

            for (var day = 1; day <= daysInPeriod; day++)
            {
                var employeeShift = employeeTimesheet.EmployeeShifts
                    .FirstOrDefault(x => x.Date.Day == day);

                if (employeeShift is not null)
                {
                    range = worksheet.Cells[rowIndex, colIndex];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(employeeShift.Shift.Color));
                    range.Value = employeeShift.Shift.Code;

                    if (data.ExportWorkHours)
                    {
                        if (employeeShift.Shift.Type == ShiftType.Workday
                            && employeeShift.Shift.StartTime.HasValue
                            && employeeShift.Shift.EndTime.HasValue)
                        {
                            range.Style.WrapText = true;
                            range.Value = employeeShift.Shift.StartTime.Value.ToString(@"hh\:mm")
                                          + " "
                                          + employeeShift.Shift.EndTime.Value.ToString(@"hh\:mm");
                        }
                    }
                }

                colIndex++;
            }

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = employeeTimesheet.TotalWorkHours;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(TotalCellColor));
            colIndex++;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = employeeTimesheet.TotalWorkDays;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(TotalCellColor));

            range = worksheet.Cells[rowIndex, 1, rowIndex, colIndex];
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            range = worksheet.Cells[rowIndex, 4, rowIndex, colIndex];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            rowIndex++;
            employeeIndex++;
        }

        #endregion

        #endregion

        rowIndex += 2;

        var shifts = data.Timesheet.EmployeeTimesheets
            .SelectMany(x => x.EmployeeShifts)
            .Select(x => x.Shift)
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.Description)
            .ToList();

        foreach (var shift in shifts)
        {
            colIndex = 1;
            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = shift.Code;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (!string.IsNullOrEmpty(shift.Color))
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(shift.Color));
            }

            colIndex++;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Merge = true;
            range.Value = shift.Description;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(TotalCellColor));
            colIndex++;

            range = worksheet.Cells[rowIndex, colIndex];
            range.Value = "";

            worksheet.Cells[rowIndex, 1, rowIndex, 2].SetRangeBorder();
            rowIndex++;
        }

        #region Formatting

        colIndex = 1;
        worksheet.Column(colIndex).Width = 5;
        colIndex++;

        worksheet.Column(colIndex).Width = 30;
        colIndex++;

        worksheet.Column(colIndex).Width = 30;
        colIndex++;

        for (var day = 0; day < daysInPeriod; day++)
        {
            worksheet.Column(colIndex).Width = 7;
            colIndex++;
        }

        worksheet.Column(colIndex).Width = 10;
        colIndex++;

        worksheet.Column(colIndex).Width = 10;

        #endregion

        return package.GetAsByteArray();
    }
}