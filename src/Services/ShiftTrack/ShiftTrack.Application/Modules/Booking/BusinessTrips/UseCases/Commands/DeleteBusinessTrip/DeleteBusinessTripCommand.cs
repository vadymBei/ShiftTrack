using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.DeleteBusinessTrip;

public record DeleteBusinessTripCommand(
    long Id) : IRequest;