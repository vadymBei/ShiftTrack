using ShiftTrack.Application.Modules.Booking.Common.Dtos;
using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Queries.GetVacations;

public record GetVacationsQuery(
    VacationsFilterDto Filter) : IRequest<IEnumerable<VacationVm>>;