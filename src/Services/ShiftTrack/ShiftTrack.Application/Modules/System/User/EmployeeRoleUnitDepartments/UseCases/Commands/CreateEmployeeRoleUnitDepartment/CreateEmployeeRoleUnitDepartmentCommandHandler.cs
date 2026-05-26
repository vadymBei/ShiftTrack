using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Commands.
    CreateEmployeeRoleUnitDepartment;

public class CreateEmployeeRoleUnitDepartmentCommandHandler(
    IMapper mapper,
    IEmployeeRoleUnitDepartmentService employeeRoleUnitDepartmentService)
    : IRequestHandler<CreateEmployeeRoleUnitDepartmentCommand, EmployeeRoleUnitDepartmentVm>
{
    public async Task<EmployeeRoleUnitDepartmentVm> Handle(CreateEmployeeRoleUnitDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await employeeRoleUnitDepartmentService.Create(
            new EmployeeRoleUnitDepartmentsToCreateDto(
                request.EmployeeRoleUnitId,
                new List<long> { request.DepartmentId }),
            cancellationToken);

        return mapper.Map<EmployeeRoleUnitDepartmentVm>(employeeRoleUnitDepartment);
    }
}