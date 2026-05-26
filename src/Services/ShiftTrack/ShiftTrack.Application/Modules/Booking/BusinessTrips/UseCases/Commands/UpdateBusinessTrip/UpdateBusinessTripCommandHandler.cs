using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Extensions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.UpdateBusinessTrip;

public class UpdateBusinessTripCommandHandler(
    IMapper mapper,
    ILocationRepository locationRepository,
    IBusinessTripService businessTripService) : IRequestHandler<UpdateBusinessTripCommand, BusinessTripVm>
{
    public async Task<BusinessTripVm> Handle(UpdateBusinessTripCommand request, CancellationToken cancellationToken = default)
    {
        var locations = await locationRepository
            .GetListByIds(request.Data.LocationIntegrationIds, cancellationToken);
        
        var locationDictionary = locations
            .ToDictionary(x => x.IntegrationId);
        
        var businessTrip = await businessTripService.Update(request.Data, cancellationToken);
        
        var businessTripVm = mapper.Map<BusinessTripVm>(businessTrip);
        businessTripVm.EnrichWithLocationData(locationDictionary);
        
        return businessTripVm;
    }
}