using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.RejectBusinessTrip;

public class RejectBusinessTripCommandHandler(
    IMapper mapper,
    IBusinessTripService businessTripService) : IRequestHandler<RejectBusinessTripCommand, BusinessTripVm>
{
    public async Task<BusinessTripVm> Handle(RejectBusinessTripCommand request, CancellationToken cancellationToken = default)
    {
        await businessTripService.Reject(request.Id, cancellationToken);
        
        var businessTrip = await businessTripService.GetById(request.Id, cancellationToken);
        
        return mapper.Map<BusinessTripVm>(businessTrip);
    }
}