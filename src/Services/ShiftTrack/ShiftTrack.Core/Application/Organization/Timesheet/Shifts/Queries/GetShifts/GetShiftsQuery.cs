using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts
{
    public class GetShiftsQuery : IRequest<IEnumerable<ShiftVM>>
    {
    }
}
