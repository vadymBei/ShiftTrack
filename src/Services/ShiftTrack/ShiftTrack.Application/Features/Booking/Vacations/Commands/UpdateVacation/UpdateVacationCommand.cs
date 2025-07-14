using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;

public record UpdateVacationCommand(
    long Id) : IRequest<VacationVm>;