using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Commands.CreateEmployeeRoleUnitDepartment;

public class CreateEmployeeRoleUnitDepartmentCommandHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<CreateEmployeeRoleUnitDepartmentCommand, EmployeeRoleUnitDepartmentVm>
{
    public async Task<EmployeeRoleUnitDepartmentVm> Handle(CreateEmployeeRoleUnitDepartmentCommand request, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await employeeRoleService.CreateEmployeeRoleUnitDepartments(
            new EmployeeRoleUnitDepartmentsToCreateDto(
                request.EmployeeRoleUnitId,
                new List<long> { request.DepartmentId }),
            cancellationToken);
        
        return mapper.Map<EmployeeRoleUnitDepartmentVm>(employeeRoleUnitDepartment);
    }
}