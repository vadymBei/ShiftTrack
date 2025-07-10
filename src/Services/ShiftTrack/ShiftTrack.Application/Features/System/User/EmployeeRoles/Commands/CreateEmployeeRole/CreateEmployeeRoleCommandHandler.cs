using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public class CreateEmployeeRoleCommandHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<CreateEmployeeRoleCommand, EmployeeRoleVm>
{
    public async Task<EmployeeRoleVm> Handle(CreateEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleService.CreateEmployeeRole(
            request.Dto,
            cancellationToken);
        
        employeeRole = await employeeRoleService.GetEmployeeRoleById(
            employeeRole.Id,
            cancellationToken);
        
        return mapper.Map<EmployeeRoleVm>(employeeRole);
    }
}