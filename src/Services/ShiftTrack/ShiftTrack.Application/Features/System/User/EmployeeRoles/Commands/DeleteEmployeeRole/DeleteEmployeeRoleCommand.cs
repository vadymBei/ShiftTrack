using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

public record DeleteEmployeeRoleCommand(long EmployeeRoleId) : IRequest;