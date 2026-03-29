using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public class GetEmployeeRoleUnitsByEmployeeRoleIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<GetEmployeeRoleUnitsByEmployeeRoleIdQuery, IEnumerable<EmployeeRoleUnitVm>>
{
    public async Task<IEnumerable<EmployeeRoleUnitVm>> Handle(GetEmployeeRoleUnitsByEmployeeRoleIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnits = await employeeRoleService.GetEmployeeRoleUnitsByEmployeeRoleId(
            request.EmployeeRoleId,
            cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeRoleUnitVm>>(employeeRoleUnits); 
    }
}