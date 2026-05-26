using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.UpdatePosition;

public class UpdatePositionCommandHandler(
    IMapper mapper,
    IPositionRepository positionRepository)
    : IRequestHandler<UpdatePositionCommand, PositionVm>
{
    public async Task<PositionVm> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
       var position = await positionRepository.Update(request.Data, cancellationToken);

        return mapper.Map<PositionVm>(position);
    }
}