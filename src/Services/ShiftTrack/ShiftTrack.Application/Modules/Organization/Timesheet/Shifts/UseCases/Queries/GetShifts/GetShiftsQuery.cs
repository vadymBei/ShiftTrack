using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShifts;

public record GetShiftsQuery() : IRequest<IEnumerable<ShiftVm>>;