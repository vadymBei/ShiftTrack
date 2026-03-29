using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;

public class CreatePositionCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreatePositionCommand, PositionVm>
{
    public async Task<PositionVm> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = new Position()
        {
            Name = request.Name,
            Description = request.Description
        };

        applicationDbContext.Positions.Add(position);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PositionVm>(position);
    }
}