using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;

public record CreateUnitCommand(
    string Name,
    string Description,
    string Code) : IRequest<UnitVM>;