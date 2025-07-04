﻿using AutoMapper;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;

public class CreatePositionCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreatePositionCommand, PositionVM>
{
    public async Task<PositionVM> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = new Position()
        {
            Name = request.Name,
            Description = request.Description
        };

        applicationDbContext.Positions.Add(position);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PositionVM>(position);
    }
}