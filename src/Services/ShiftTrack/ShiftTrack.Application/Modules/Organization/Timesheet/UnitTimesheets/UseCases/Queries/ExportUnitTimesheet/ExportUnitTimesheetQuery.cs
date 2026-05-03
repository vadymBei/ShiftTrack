using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.ExportUnitTimesheet;

public record ExportUnitTimesheetQuery(
    DateTime Period,
    long DepartmentId,
    bool ExportWorkHours) : IRequest<DocumentVm>;