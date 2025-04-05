using MediatR;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Constants;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public class CreateEmployeeRoleCommandHandler(
    IEmployeeService employeeService,
    ICurrentUserService currentUserService,
    IEmployeeRoleService employeeRoleService)
    : IRequestHandler<CreateEmployeeRoleCommand>
{
    public async Task<Unit> Handle(CreateEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        var canUse = currentUserService.User.Roles
            .Any(x => x == DefaultRolesCatalog.Sys_Admin
                      || x == DefaultRolesCatalog.Regional_Director
                      || x == DefaultRolesCatalog.Department_Director);

        if (!canUse)
        {
            throw new AccessDeniedException(
                new List<string>
                {
                    DefaultRolesCatalog.Sys_Admin,
                    DefaultRolesCatalog.Regional_Director,
                    DefaultRolesCatalog.Department_Director
                });
        }

        var employee = await employeeService
            .GetById(request.EmployeeId, cancellationToken);

        await employeeRoleService.CreateEmployeeRole(
            new EmployeeRoleToCreateDto(
                employee.IntegrationId,
                request.RoleId)
            , cancellationToken);

        return Unit.Value;
    }
}