using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

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