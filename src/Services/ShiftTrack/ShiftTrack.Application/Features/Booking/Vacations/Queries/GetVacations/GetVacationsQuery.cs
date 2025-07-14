using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public record GetVacationsQuery(
    DateTime DateFrom,
    DateTime DateTo,
    long UnitId,
    long DepartmentId) : IRequest<IEnumerable<VacationVm>>;