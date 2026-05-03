using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.UpdateUnit;

public record UpdateUnitCommand(
    UnitToUpdateDto Data) : IRequest<UnitVm>;