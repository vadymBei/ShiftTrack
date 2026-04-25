using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Commands.UpdatePosition;

public record UpdatePositionCommand(
    long Id,
    string Name,
    string Description,
    decimal HourlyRate) : IRequest<PositionVm>;