using MediatR;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public record CreateEmployeeRoleCommand(
    long EmployeeId,
    string RoleId) : IRequest;