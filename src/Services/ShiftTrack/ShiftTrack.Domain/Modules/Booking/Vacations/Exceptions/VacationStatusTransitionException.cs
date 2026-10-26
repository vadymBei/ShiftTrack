using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Domain.Modules.Booking.Vacations.Exceptions;

/// <summary>A status change the vacation state machine doesn't allow.</summary>
public class VacationStatusTransitionException(VacationStatus status, string action)
    : InvalidOperationException($"Vacation in status {status} can't be {action}.");
