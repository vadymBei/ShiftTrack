using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.CreateBusinessTrip;

public record CreateBusinessTripCommand(
    BusinessTripToCreateDto Data) : IRequest<BusinessTripVm>;