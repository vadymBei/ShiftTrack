using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Queries.GetEmployeeRolesByEmployeeId;

public class GetEmployeeRolesByEmployeeIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleRepository employeeRoleRepository) : IRequestHandler<GetEmployeeRolesByEmployeeIdQuery, IEnumerable<EmployeeRoleVm>>
{
    public async Task<IEnumerable<EmployeeRoleVm>> Handle(GetEmployeeRolesByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoles = await employeeRoleRepository.GetListByEmployeeId(
            request.EmployeeId,
            cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeRoleVm>>(employeeRoles);
    }
}