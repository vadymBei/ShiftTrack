using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.UpdatePosition;

public class UpdatePositionCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdatePositionCommand, PositionVm>
{
    public async Task<PositionVm> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
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

        return mapper.Map<PositionVm>(position);
    }
}