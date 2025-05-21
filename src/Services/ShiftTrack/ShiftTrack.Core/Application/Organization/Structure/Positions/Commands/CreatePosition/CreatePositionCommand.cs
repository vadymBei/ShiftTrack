using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;

public record CreatePositionCommand(
    string Name,
    string Description) : IRequest<PositionVM>;