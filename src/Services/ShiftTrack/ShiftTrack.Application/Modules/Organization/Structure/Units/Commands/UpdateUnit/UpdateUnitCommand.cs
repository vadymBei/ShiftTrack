using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    long Id,
    string Name,
    string Description,
    string Code) : IRequest<UnitVm>;