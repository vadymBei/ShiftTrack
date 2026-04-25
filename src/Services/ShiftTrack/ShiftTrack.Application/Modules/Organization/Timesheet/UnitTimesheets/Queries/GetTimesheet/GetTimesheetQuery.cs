using ShiftTrack.Application.Modules.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Queries.GetTimesheet;

public record GetTimesheetQuery(
    TimesheetDto Dto) : IRequest<TimesheetVm>;