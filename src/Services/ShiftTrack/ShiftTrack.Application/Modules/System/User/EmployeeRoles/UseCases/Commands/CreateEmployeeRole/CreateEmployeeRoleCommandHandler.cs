using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Commands.CreateEmployeeRole;

public class CreateEmployeeRoleCommandHandler(
    IMapper mapper,
    IRoleRepository roleRepository,
    IUnitRepository unitRepository,
    IEmployeeRoleService employeeRoleService,
    IDepartmentRepository departmentRepository,
    IEmployeeRoleRepository employeeRoleRepository,
    IEmployeeRoleUnitService employeeRoleUnitService,
    IEmployeeRoleUnitDepartmentService employeeRoleUnitDepartmentService)
    : IRequestHandler<CreateEmployeeRoleCommand, EmployeeRoleVm>
{
    public async Task<EmployeeRoleVm> Handle(CreateEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        await roleRepository.GetById(request.Dto.RoleId, cancellationToken);

        var employeeRole = await employeeRoleService.Create(
            request.Dto,
            cancellationToken);

        if (request.Dto.UnitId is not null)
        {
            await unitRepository.GetById((long)request.Dto.UnitId, cancellationToken);

            var employeeRoleUnit = await employeeRoleUnitService.Create(
                new EmployeeRoleUnitToCreateDto(
                    employeeRole.Id,
                    (long)request.Dto.UnitId,
                    RoleScope.Local),
                cancellationToken);

            var departments = await departmentRepository.GetByIds(
                request.Dto.DepartmentIds,
                cancellationToken);

            if (departments.Any())
            {
                await employeeRoleUnitDepartmentService.Create(
                    new EmployeeRoleUnitDepartmentsToCreateDto(
                        employeeRoleUnit.Id,
                        departments.Select(x => x.Id)),
                    cancellationToken);
            }
        }

        employeeRole = await employeeRoleRepository.GetById(
            employeeRole.Id,
            cancellationToken);

        return mapper.Map<EmployeeRoleVm>(employeeRole);
    }
}