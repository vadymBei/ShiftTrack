using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.ExportUnitTimesheet;

public record UnitTimesheetExportData(
    UnitTimesheet Timesheet,
    bool ExportWorkHours);