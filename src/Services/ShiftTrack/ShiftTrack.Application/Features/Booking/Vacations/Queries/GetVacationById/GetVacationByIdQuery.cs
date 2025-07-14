using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacationById;

public record GetVacationByIdQuery(
    long Id) : IRequest<VacationVm>;