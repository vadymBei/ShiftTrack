using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;

public record TimesheetExportData(
    UnitTimesheet Timesheet,
    bool ExportWorkHours);