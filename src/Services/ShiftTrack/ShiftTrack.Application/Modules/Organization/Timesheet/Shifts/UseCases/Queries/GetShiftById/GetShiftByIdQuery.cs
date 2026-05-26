using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShiftById;

public record GetShiftByIdQuery(
    long Id) : IRequest<ShiftVm>;