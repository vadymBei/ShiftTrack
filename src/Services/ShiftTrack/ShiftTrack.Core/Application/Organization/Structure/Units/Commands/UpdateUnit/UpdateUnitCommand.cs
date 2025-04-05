using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    long Id,
    string Name,
    string Description,
    string Code) : IRequest<UnitVM>;