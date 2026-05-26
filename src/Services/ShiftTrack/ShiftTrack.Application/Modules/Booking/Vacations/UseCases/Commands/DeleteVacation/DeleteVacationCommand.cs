using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.DeleteVacation;

public record DeleteVacationCommand(
    long Id) : IRequest;