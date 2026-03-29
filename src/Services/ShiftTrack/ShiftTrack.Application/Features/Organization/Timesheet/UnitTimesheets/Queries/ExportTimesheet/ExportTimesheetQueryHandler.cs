using ShiftTrack.Application.Common.Constants;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;

public class ExportTimesheetQueryHandler(
    IExcelExporter<TimesheetExportData> timesheetPlanExporter,
    ITimesheetService timesheetService) : IRequestHandler<ExportTimesheetQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(ExportTimesheetQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await timesheetService.GetTimesheet(
            new TimesheetDto(
                request.Period,
                request.DepartmentId),
            cancellationToken);

        var content = timesheetPlanExporter.Export(
            new TimesheetExportData(
                timesheet,
                request.ExportWorkHours));

        return new DocumentVm()
        {
            Content = content,
            Extension = FileExtensions.Excel,
            Name = "Timesheet"
        };
    }
}