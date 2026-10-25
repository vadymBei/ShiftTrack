using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Events;

public record VacationCreatedEvent(
    long VacationId) : IDomainEvent;
