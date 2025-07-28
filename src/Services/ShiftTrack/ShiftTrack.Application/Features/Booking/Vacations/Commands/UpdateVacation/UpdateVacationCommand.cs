using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;

public record UpdateVacationCommand(
    long Id,
    DateTime StartDate,
    DateTime EndDate,
    string Comment,
    VacationType Type) : IRequest<VacationVm>;