using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.UpdateVacation;

public record UpdateVacationCommand(
    long Id,
    DateTime StartDate,
    DateTime EndDate,
    string Comment,
    VacationType Type) : IRequest<VacationVm>;