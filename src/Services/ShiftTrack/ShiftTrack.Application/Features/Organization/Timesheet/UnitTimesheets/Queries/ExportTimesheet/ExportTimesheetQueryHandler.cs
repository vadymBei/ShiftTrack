using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;

public class ExportTimesheetQueryHandler(
    IExcelGenerator excelGenerator,
    IExcelFormatter<TimesheetExportData> timesheetPlanFormatter,
    ITimesheetService timesheetService) : IRequestHandler<ExportTimesheetQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(ExportTimesheetQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await timesheetService.GetTimesheet(
            new TimesheetDto(
                request.Period,
                request.DepartmentId),
            cancellationToken);

        var content = excelGenerator.Generate<TimesheetExportData>(
            timesheetPlanFormatter,
            new TimesheetExportData(
                timesheet,
                request.ExportWorkHours));

        return new DocumentVm()
        {
            Content = content,
            Extension = ".xlsx",
            Name = "Timesheet"
        };
    }
}