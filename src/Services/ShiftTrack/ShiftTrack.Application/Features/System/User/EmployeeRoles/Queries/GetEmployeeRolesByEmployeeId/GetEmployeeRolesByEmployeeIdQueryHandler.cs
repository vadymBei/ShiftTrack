using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

public class GetEmployeeRolesByEmployeeIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<GetEmployeeRolesByEmployeeIdQuery, IEnumerable<EmployeeRoleVm>>
{
    public async Task<IEnumerable<EmployeeRoleVm>> Handle(GetEmployeeRolesByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoles = await employeeRoleService.GetEmployeeRolesByEmployeeId(
            request.EmployeeId,
            cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeRoleVm>>(employeeRoles);
    }
}