using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.DownloadBusinessTripOrderPdf;

public record DownloadBusinessTripOrderQuery(
    long Id) : IRequest<DocumentVm>;