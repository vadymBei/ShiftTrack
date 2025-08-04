using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.DeleteVacation;

public record DeleteVacationCommand(
    long Id) : IRequest;