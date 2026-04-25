using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.RejectVacation;

public record RejectVacationCommand(
    long Id) : IRequest<VacationVm>;