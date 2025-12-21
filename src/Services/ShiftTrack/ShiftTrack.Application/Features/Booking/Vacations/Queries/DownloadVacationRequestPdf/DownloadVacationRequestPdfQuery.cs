using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.DownloadVacationRequestPdf;

public record DownloadVacationRequestPdfQuery(
    long VacationId) : IRequest<DocumentVm>;