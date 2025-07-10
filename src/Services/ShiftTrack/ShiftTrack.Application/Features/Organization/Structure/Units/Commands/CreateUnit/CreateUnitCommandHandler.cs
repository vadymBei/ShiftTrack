using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;

public class CreateUnitCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<CreateUnitCommand, UnitVm>
{
    public async Task<UnitVm> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = new Unit()
        {
            Name = request.Name,
            Description = request.Description,
            Code = request.Code
        };

        await dbContext.Units.AddAsync(unit, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}