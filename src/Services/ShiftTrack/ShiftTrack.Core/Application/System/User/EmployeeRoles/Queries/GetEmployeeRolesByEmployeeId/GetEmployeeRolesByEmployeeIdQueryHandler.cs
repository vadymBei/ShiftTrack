using AutoMapper;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

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