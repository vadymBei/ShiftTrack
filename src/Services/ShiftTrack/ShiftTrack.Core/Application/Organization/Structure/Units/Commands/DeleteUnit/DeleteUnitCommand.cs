using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit;

public record DeleteUnitCommand(
    long Id) : IRequest;