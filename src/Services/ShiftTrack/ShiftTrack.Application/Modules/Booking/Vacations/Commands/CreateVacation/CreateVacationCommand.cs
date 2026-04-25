using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.CreateVacation;

public record CreateVacationCommand(
    DateTime StartDate,
    DateTime EndDate,
    string Comment,
    long EmployeeId,
    VacationType Type) : IRequest<VacationVm>;