using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.DownloadBusinessTripOrderPdf;

public class DownloadBusinessTripOrderQueryHandler(
    ILocationRepository locationRepository,
    IBusinessTripService businessTripService,
    IPdfExporter<BusinessTripOrderData> businessTripOrderExporter)
    : IRequestHandler<DownloadBusinessTripOrderQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(DownloadBusinessTripOrderQuery request, CancellationToken cancellationToken = default)
    {
        var businessTrip = await businessTripService.GetById(request.Id, cancellationToken);

        var locations = await locationRepository
            .GetListByIds(businessTrip.Locations.Select(x => x.LocationIntegrationId), cancellationToken);
        
        var streamContent = await businessTripOrderExporter.Export(
            new BusinessTripOrderData()
            {
                BusinessTrip = businessTrip,
                Locations = locations
            },
            cancellationToken);

        return new DocumentVm()
        {
            StreamContent = streamContent,
            Extension = ".pdf",
            Name = $"business_trip_order_{businessTrip.Id}"
        };
    }
}