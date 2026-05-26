using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.RejectBusinessTrip;

public record RejectBusinessTripCommand(
    long Id) : IRequest<BusinessTripVm>;