using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositions;

public class GetPositionsQueryHandler(
    IMapper mapper,
    IPositionRepository positionRepository)
    : IRequestHandler<GetPositionsQuery, IEnumerable<PositionVm>>
{
    public async Task<IEnumerable<PositionVm>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await positionRepository.GetAll(cancellationToken);

        return mapper.Map<List<PositionVm>>(positions);
    }
}