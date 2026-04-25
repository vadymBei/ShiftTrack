using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.ApproveVacation;

public record ApproveVacationCommand(
    long Id) : IRequest<VacationVm>;