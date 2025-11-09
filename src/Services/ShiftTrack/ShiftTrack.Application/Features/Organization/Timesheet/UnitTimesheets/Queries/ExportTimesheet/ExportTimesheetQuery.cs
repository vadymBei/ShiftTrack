using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;

public record ExportTimesheetQuery(
    DateTime Period,
    long DepartmentId,
    bool ExportWorkHours) : IRequest<DocumentVm>;