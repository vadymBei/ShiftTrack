using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public class CreateEmployeeRoleUnitCommandHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<CreateEmployeeRoleUnitCommand, EmployeeRoleUnitVm>
{
    public async Task<EmployeeRoleUnitVm> Handle(CreateEmployeeRoleUnitCommand request, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleService.CreateEmployeeRoleUnit(
            new EmployeeRoleUnitToCreateDto(
                request.EmployeeRoleId,
                request.UnitId,
                RoleScope.Global),
            cancellationToken);
        
        return mapper.Map<EmployeeRoleUnitVm>(employeeRole);
    }
}