using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;

public class CreatePositionCommandHandler(
    IMapper mapper,
    IPositionRepository positionRepository)
    : IRequestHandler<CreatePositionCommand, PositionVm>
{
    public async Task<PositionVm> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await positionRepository.Create(request.Data, cancellationToken);

        return mapper.Map<PositionVm>(position);
    }
}