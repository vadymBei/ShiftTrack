using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacationById;

public record GetVacationByIdQuery(
    long Id) : IRequest<VacationVm>;