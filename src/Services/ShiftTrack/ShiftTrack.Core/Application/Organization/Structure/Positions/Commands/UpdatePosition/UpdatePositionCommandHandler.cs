using AutoMapper;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.UpdatePosition;

public class UpdatePositionCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdatePositionCommand, PositionVM>
{
    public async Task<PositionVM> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await applicationDbContext.Positions
            .FindAsync(request.Id);

        if (position is null)
        {
            throw new EntityNotFoundException(typeof(Position), request.Id);
        }

        position.Name = request.Name;
        position.Description = request.Description;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PositionVM>(position);
    }
}