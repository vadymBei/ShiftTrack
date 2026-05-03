using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.DeleteEmployeeRoleUnit;

public record DeleteEmployeeRoleUnitCommand(
    long Id) : IRequest;