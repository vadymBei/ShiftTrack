using ShiftTrack.Application.Common.Constants;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.ExportUnitTimesheet;

public class ExportUnitTimesheetQueryHandler(
    IExcelExporter<UnitTimesheetExportData> timesheetPlanExporter,
    IUnitTimesheetService unitTimesheetService) : IRequestHandler<ExportUnitTimesheetQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(ExportUnitTimesheetQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await unitTimesheetService.GetTimesheet(
            new UnitTimesheetDto(
                request.Period,
                request.DepartmentId),
            cancellationToken);

        var content = timesheetPlanExporter.Export(
            new UnitTimesheetExportData(
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