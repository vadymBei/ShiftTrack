using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.CreateVacation;

public record CreateVacationCommand(
    DateTime StartDate,
    DateTime EndDate,
    string Comment,
    long EmployeeId,
    VacationType Type) : IRequest<VacationVm>;