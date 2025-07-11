using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.Register;
using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Data.Initializations.Commands.InitializeSysAdmin;

public sealed class InitializeSysAdminCommandHandler(
    IMediator mediator,
    IEmployeeRoleService employeeRoleService,
    IApplicationDbContext applicationDbContext) : IRequestHandler<InitializeSysAdminCommand>
{
    public async Task Handle(InitializeSysAdminCommand request, CancellationToken cancellationToken = default)
    {
        var sysAdminExisted = await applicationDbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == "+380977450521", cancellationToken);

        if (sysAdminExisted is null)
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

            var sysAdminRole = await applicationDbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == DefaultRolesCatalog.SYS_ADMIN, cancellationToken);

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