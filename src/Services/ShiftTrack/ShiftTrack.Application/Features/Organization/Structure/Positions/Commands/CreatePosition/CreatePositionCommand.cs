using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;

public record CreatePositionCommand(
    string Name,
    string Description) : IRequest<PositionVm>;