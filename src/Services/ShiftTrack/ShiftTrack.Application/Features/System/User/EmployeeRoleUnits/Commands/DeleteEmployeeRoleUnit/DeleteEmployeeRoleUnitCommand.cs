using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Commands.DeleteEmployeeRoleUnit;

public record DeleteEmployeeRoleUnitCommand(
    long Id) : IRequest;