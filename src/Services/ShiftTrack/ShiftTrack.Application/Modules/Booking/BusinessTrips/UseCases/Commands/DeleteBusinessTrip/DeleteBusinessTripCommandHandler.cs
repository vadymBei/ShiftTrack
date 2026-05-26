using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.DeleteBusinessTrip;

public class DeleteBusinessTripCommandHandler(
    IBusinessTripService businessTripService) : IRequestHandler<DeleteBusinessTripCommand>
{
    public async Task Handle(DeleteBusinessTripCommand request, CancellationToken cancellationToken = default)
    {
        await businessTripService.Delete(request.Id, cancellationToken);
    }
}