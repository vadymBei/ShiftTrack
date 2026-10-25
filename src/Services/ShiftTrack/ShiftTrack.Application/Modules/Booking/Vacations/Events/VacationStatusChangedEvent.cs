using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Events;

public record VacationStatusChangedEvent(
    long VacationId,
    VacationStatus OldStatus,
    VacationStatus NewStatus) : IDomainEvent;
