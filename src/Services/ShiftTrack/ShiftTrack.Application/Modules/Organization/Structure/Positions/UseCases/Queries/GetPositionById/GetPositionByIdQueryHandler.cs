using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositionById;

public class GetPositionByIdQueryHandler(
    IMapper mapper,
    IPositionRepository positionRepository) : IRequestHandler<GetPositionByIdQuery, PositionVm>
{
    public async Task<PositionVm> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var position = await positionRepository.GetById(request.Id, cancellationToken);

        return mapper.Map<PositionVm>(position);
    }
}