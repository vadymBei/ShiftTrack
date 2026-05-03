using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;

public record CreatePositionCommand(
    PositionToCreateDto Data) : IRequest<PositionVm>;