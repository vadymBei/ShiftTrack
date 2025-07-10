using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShiftById;

public record GetShiftByIdQuery(
    long Id) : IRequest<ShiftVm>;