using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitById;

public class GetUnitByIdQueryHandler(
    IMapper mapper,
    IUnitRepository unitRepository) : IRequestHandler<GetUnitByIdQuery, UnitVm>
{
    public async Task<UnitVm> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.GetById(request.Id, cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}