using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

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