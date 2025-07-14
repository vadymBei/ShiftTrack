using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.ApproveVacation;

public record ApproveVacationCommand(
    long Id) : IRequest<VacationVm>;