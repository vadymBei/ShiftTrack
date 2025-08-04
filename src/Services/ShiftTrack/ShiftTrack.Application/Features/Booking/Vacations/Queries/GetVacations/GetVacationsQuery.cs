using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public record GetVacationsQuery(
    VacationsFilterDto Filter) : IRequest<IEnumerable<VacationVm>>;