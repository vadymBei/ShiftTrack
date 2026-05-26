using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.GetUnitTimesheet;

public record GetTimesheetQuery(
    UnitTimesheetDto Dto) : IRequest<UnitTimesheetVm>;