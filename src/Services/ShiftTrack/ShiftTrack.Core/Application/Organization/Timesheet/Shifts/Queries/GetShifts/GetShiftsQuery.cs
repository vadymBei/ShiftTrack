using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;

public record GetShiftsQuery() : IRequest<IEnumerable<ShiftVM>>;