using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public record GetVacationsQuery(
    DateTime? StartDate,
    DateTime? EndDate,
    long UnitId,
    long DepartmentId,
    VacationStatus Status,
    string SearchPattern) : IRequest<IEnumerable<VacationVm>>;