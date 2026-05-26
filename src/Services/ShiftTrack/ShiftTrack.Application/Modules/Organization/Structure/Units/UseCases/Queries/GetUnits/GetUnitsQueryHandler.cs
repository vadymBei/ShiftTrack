using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnits;

public class GetUnitsQueryHandler(
    IMapper mapper,
    IUnitRepository unitRepository) : IRequestHandler<GetUnitsQuery, IEnumerable<UnitVm>>
{
    public async Task<IEnumerable<UnitVm>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await unitRepository.GetAll(cancellationToken);

        return mapper.Map<List<UnitVm>>(units);
    }
}