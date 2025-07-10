using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;

public record CreateUnitCommand(
    string Name,
    string Description,
    string Code) : IRequest<UnitVm>;