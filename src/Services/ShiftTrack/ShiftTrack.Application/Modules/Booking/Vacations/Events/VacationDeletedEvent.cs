using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Events;

public record VacationDeletedEvent(
    long VacationId,
    long EmployeeId,
    long? AuthorId,
    VacationType Type,
    DateTime StartDate,
    DateTime EndDate) : IDomainEvent;
