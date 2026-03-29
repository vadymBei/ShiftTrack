using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

public class GetEmployeeRoleUnitByIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService) : IRequestHandler<GetEmployeeRoleUnitByIdQuery, EmployeeRoleUnitVm>
{
    public async Task<EmployeeRoleUnitVm> Handle(GetEmployeeRoleUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleService.GetEmployeeRoleUnitById(
            request.Id,
            cancellationToken);
        
        return mapper.Map<EmployeeRoleUnitVm>(employeeRoleUnit);
    }
}