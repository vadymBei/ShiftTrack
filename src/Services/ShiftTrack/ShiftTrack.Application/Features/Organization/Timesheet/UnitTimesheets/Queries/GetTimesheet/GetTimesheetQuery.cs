using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.GetTimesheet;

public record GetTimesheetQuery(
    TimesheetDto Dto) : IRequest<TimesheetVm>;