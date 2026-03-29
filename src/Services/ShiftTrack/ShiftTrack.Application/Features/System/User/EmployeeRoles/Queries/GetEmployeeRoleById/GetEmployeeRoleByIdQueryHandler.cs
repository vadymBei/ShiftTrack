using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

public class GetEmployeeRoleByIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<GetEmployeeRoleByIdQuery, EmployeeRoleVm>
{
    public async Task<EmployeeRoleVm> Handle(GetEmployeeRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleService.GetEmployeeRoleById(
            request.EmployeeRoleId,
            cancellationToken);
        
        return mapper.Map<EmployeeRoleVm>(employeeRole);
    }
}