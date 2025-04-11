using MediatR;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Constants;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
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
        var allowedRoles = new HashSet<string>()
        {
            DefaultRolesCatalog.SYS_ADMIN,
            DefaultRolesCatalog.UNIT_DIRECTOR,
            DefaultRolesCatalog.DEPARTMENT_DIRECTOR
        };
        
        var canUse = currentUserService.User.Roles
            .Any(x => allowedRoles.Contains(x));

        if (!canUse)
        {
            throw new AccessDeniedException(
                allowedRoles.ToList());
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