using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.Register;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Data.Initializations.UseCases.Commands.InitializeSysAdmin;

public sealed class InitializeSysAdminCommandHandler(
    IMediator mediator,
    IRoleRepository roleRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<InitializeSysAdminCommand>
{
    public async Task Handle(InitializeSysAdminCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetByPhoneNumber("+380977450521", cancellationToken);

        if (employee is null)
        {
            var createEmployeeCommand = new RegisterCommand(
                "Vadym",
                "Bei",
                "Ihorovych",
                "bey1705@gmail.com",
                "5nAEz@Jw8KR4Rf9",
                "5nAEz@Jw8KR4Rf9",
                "+380977450521",
                EmployeeGender.Male);

            var sysAdmin = await mediator.Invoke(createEmployeeCommand, cancellationToken);

            var sysAdminRole = await roleRepository.GetByName(DefaultRolesCatalog.SYS_ADMIN, cancellationToken);
            
            if (sysAdminRole is not null)
            {
                await employeeRoleService.CreateSysAdminEmployeeRole(
                    new EmployeeRoleToCreateDto(
                        sysAdmin.Id,
                        sysAdminRole.Id,
                        null,
                        []),
                    cancellationToken);
            }
        }
    }
}