using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

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