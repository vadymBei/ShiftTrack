using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Commands.DeleteUnit;

public record DeleteUnitCommand(
    long Id) : IRequest;