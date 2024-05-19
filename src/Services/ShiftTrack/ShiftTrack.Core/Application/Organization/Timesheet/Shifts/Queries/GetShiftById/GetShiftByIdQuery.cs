using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById
{
    public class GetShiftByIdQuery : IRequest<ShiftVM>
    {
        public long Id { get; set; }
    }
}
