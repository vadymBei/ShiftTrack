using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.CreateEmployeeRoleUnit;

public class CreateEmployeeRoleUnitCommandHandler(
    IMapper mapper,
    IEmployeeRoleUnitService employeeRoleUnitService)
    : IRequestHandler<CreateEmployeeRoleUnitCommand, EmployeeRoleUnitVm>
{
    public async Task<EmployeeRoleUnitVm> Handle(CreateEmployeeRoleUnitCommand request,
        CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleUnitService.Create(
            new EmployeeRoleUnitToCreateDto(
                request.EmployeeRoleId,
                request.UnitId,
                RoleScope.Global),
            cancellationToken);

        return mapper.Map<EmployeeRoleUnitVm>(employeeRole);
    }
}