namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;

public record BusinessTripFilterDto(
    DateTime StartDate,
    DateTime EndDate,
    long DepartmentId,
    string SearchPattern);