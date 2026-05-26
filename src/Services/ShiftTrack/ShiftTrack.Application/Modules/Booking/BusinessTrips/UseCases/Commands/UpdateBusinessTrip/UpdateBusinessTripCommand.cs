using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.UpdateBusinessTrip;

public record UpdateBusinessTripCommand(
    BusinessTripToUpdateDto Data) : IRequest<BusinessTripVm>;