using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit;

public class UpdateUnitCommandHandler(
    IMapper mapper,
    IUnitService unitService,
    IApplicationDbContext dbContext)
    : IRequestHandler<UpdateUnitCommand, UnitVM>
{
    public async Task<UnitVM> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await dbContext.Units
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(Domain.Organization.Structure.Entities.Unit), request.Id);

        unit.Name = request.Name;
        unit.Description = request.Description;
        unit.Code = request.Code;

        await dbContext.SaveChangesAsync(cancellationToken);

        unit = await unitService.GetById(request.Id, cancellationToken);

        return mapper.Map<UnitVM>(unit);
    }
}