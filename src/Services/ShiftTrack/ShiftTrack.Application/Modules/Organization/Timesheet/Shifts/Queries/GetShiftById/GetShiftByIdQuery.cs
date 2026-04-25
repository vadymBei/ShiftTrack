using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Queries.GetShiftById;

public record GetShiftByIdQuery(
    long Id) : IRequest<ShiftVm>;