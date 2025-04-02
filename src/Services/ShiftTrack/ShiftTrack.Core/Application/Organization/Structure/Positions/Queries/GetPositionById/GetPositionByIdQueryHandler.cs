using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById;

public class GetPositionByIdQueryHandler(
    IMapper mapper,
    IPositionService positionService) : IRequestHandler<GetPositionByIdQuery, PositionVM>
{
    public async Task<PositionVM> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var position = await positionService.GetById(request.Id, cancellationToken);

        return mapper.Map<PositionVM>(position);
    }
}