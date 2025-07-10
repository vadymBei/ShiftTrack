using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Commands.UpdateUnit;

public class UpdateUnitCommandHandler(
    IMapper mapper,
    IUnitService unitService,
    IApplicationDbContext dbContext)
    : IRequestHandler<UpdateUnitCommand, UnitVm>
{
    public async Task<UnitVm> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await dbContext.Units
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(Unit), request.Id);

        unit.Name = request.Name;
        unit.Description = request.Description;
        unit.Code = request.Code;

        await dbContext.SaveChangesAsync(cancellationToken);

        unit = await unitService.GetById(request.Id, cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}