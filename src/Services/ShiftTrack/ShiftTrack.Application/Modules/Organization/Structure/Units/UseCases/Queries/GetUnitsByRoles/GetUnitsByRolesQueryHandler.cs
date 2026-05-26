using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitsByRoles;

public class GetUnitsByRolesQueryHandler(
    IMapper mapper,
    IUnitService unitService) : IRequestHandler<GetUnitsByRolesQuery, IEnumerable<UnitVm>>
{
    public async Task<IEnumerable<UnitVm>> Handle(GetUnitsByRolesQuery request,
        CancellationToken cancellationToken = default)
    {
        var units = await unitService.GetUnitsByRoles(cancellationToken);

        return mapper.Map<IEnumerable<UnitVm>>(units);
    }
}