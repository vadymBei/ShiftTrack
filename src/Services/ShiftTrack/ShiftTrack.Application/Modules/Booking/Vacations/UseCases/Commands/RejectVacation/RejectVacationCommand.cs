using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RejectVacation;

public record RejectVacationCommand(
    long Id) : IRequest<VacationVm>;