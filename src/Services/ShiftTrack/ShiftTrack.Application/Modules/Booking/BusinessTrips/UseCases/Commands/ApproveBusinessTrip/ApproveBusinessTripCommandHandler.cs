using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.ApproveBusinessTrip;

public class ApproveBusinessTripCommandHandler(
    IMapper mapper,
    IBusinessTripService businessTripService) : IRequestHandler<ApproveBusinessTripCommand, BusinessTripVm>
{
    public async Task<BusinessTripVm> Handle(ApproveBusinessTripCommand request, CancellationToken cancellationToken = default)
    {
        await businessTripService.Approve(request.Id, cancellationToken);
        
        var businessTrip = await businessTripService.GetById(request.Id, cancellationToken);
        
        return mapper.Map<BusinessTripVm>(businessTrip);
    }
}