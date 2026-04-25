using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Queries.GetShifts;

public record GetShiftsQuery() : IRequest<IEnumerable<ShiftVm>>;