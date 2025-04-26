using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

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