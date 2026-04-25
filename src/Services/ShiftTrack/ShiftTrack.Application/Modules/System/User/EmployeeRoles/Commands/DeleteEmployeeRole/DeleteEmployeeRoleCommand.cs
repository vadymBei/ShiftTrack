using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

public record DeleteEmployeeRoleCommand(long EmployeeRoleId) : IRequest;