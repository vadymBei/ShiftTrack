using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacationBalanceByEmployeeId;

public record GetVacationBalanceByEmployeeIdQuery(
    long EmployeeId) : IRequest<VacationBalanceVm>;