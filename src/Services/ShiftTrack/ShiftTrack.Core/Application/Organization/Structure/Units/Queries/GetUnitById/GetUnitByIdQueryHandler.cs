using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById;

public class GetUnitByIdQueryHandler(
    IMapper mapper,
    IUnitService unitService) : IRequestHandler<GetUnitByIdQuery, UnitVM>
{
    public async Task<UnitVM> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitService.GetById(request.Id, cancellationToken);

        return mapper.Map<UnitVM>(unit);
    }
}