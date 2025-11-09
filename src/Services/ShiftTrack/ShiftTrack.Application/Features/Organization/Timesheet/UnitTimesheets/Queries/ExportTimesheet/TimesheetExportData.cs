using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;

public record TimesheetExportData(
    UnitTimesheet Timesheet,
    bool ExportWorkHours);