using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Queries.GetPositionById;

public class GetPositionByIdQueryHandler(
    IMapper mapper,
    IPositionService positionService) : IRequestHandler<GetPositionByIdQuery, PositionVm>
{
    public async Task<PositionVm> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var position = await positionService.GetById(request.Id, cancellationToken);

        return mapper.Map<PositionVm>(position);
    }
}