using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Commands.CreatePosition;

public record CreatePositionCommand(
    string Name,
    string Description,
    decimal HourlyRate) : IRequest<PositionVm>;