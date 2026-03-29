using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitById;

public class GetUnitByIdQueryHandler(
    IMapper mapper,
    IUnitService unitService) : IRequestHandler<GetUnitByIdQuery, UnitVm>
{
    public async Task<UnitVm> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitService.GetById(request.Id, cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}