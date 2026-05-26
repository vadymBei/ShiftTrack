using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Extensions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.CreateBusinessTrip;

public class CreateBusinessTripCommandHandler(
    IMapper mapper,
    ILocationRepository locationRepository,
    IBusinessTripService businessTripService) : IRequestHandler<CreateBusinessTripCommand, BusinessTripVm>
{
    public async Task<BusinessTripVm> Handle(CreateBusinessTripCommand request,
        CancellationToken cancellationToken = default)
    {
        var locations = await locationRepository
            .GetListByIds(request.Data.LocationIntegrationIds, cancellationToken);

        var businessTrip = await businessTripService
            .Create(request.Data, cancellationToken);

        var businessTripVm = mapper.Map<BusinessTripVm>(businessTrip);
        
        businessTripVm.EnrichWithLocationData(
            locations
                .ToDictionary(x => x.IntegrationId));

        return businessTripVm;
    }
}