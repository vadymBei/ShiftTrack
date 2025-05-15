using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

public record DeleteEmployeeRoleCommand(long EmployeeRoleId) : IRequest;