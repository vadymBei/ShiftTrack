using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTrips;

public record GetBusinessTripsQuery(
    BusinessTripFilterDto Filter) : IRequest<IEnumerable<BusinessTripVm>>;