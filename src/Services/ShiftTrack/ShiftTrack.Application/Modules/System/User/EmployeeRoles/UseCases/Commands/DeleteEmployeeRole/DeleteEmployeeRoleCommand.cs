using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Commands.DeleteEmployeeRole;

public record DeleteEmployeeRoleCommand(long EmployeeRoleId) : IRequest;