
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.DeleteEmployeeRoleUnit;

public record DeleteEmployeeRoleUnitCommand(
    long Id) : IRequest;