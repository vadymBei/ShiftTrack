using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.SearchLocations;

public record SearchLocationsQuery(
    string SearchTerm) : IRequest<IEnumerable<LocationVm>>;
