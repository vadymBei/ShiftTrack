using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById
{
    public record GetShiftByIdQuery(
        long Id) : IRequest<ShiftVM>;
}
