using AutoMapper;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;
using UnitEntity = ShiftTrack.Core.Domain.Organization.Structure.Entities.Unit;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;

public class CreateUnitCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<CreateUnitCommand, UnitVM>
{
    public async Task<UnitVM> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = new UnitEntity()
        {
            Name = request.Name,
            Description = request.Description,
            Code = request.Code
        };

        await dbContext.Units.AddAsync(unit, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<UnitVM>(unit);
    }
}