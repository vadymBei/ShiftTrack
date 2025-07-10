using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.UpdatePosition;

public record UpdatePositionCommand(
    long Id,
    string Name,
    string Description) : IRequest<PositionVm>;