using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTripById;

public record GetBusinessTripByIdQuery(
    long Id) : IRequest<BusinessTripVm>;