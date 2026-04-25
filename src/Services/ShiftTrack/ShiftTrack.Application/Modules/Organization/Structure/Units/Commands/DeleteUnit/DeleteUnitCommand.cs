using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Commands.DeleteUnit;

public record DeleteUnitCommand(
    long Id) : IRequest;