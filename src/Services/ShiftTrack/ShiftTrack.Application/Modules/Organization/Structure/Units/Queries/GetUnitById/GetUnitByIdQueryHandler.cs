using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Queries.GetUnitById;

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