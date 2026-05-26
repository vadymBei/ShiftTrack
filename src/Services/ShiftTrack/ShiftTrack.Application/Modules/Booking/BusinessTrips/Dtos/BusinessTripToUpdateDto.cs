namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;

public record BusinessTripToUpdateDto(
    long Id,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    decimal EstimatedBudget,
    IEnumerable<long> EmployeeIds,
    IEnumerable<string> LocationIntegrationIds);