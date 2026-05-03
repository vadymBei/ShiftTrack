using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.DeleteShift;

public class DeleteShiftCommandHandler(
    IShiftRepository shiftRepository) : IRequestHandler<DeleteShiftCommand>
{
    public async Task Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        await shiftRepository.Delete(request.Id, cancellationToken);
    }
}