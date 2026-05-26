using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.ApproveBusinessTrip;

public record ApproveBusinessTripCommand(
    long Id) : IRequest<BusinessTripVm>;