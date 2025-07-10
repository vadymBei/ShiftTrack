using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShifts;

public record GetShiftsQuery() : IRequest<IEnumerable<ShiftVm>>;