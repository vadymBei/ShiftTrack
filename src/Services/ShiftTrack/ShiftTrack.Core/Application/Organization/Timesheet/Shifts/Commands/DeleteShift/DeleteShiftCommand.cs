using MediatR;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift
{
    public class DeleteShiftCommand : IRequest
    {
        public long Id { get; set; }
    }
}
