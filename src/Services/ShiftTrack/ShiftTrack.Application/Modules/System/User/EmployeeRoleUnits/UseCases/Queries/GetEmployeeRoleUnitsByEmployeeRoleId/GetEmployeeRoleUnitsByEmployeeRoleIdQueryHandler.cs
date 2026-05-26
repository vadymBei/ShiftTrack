using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public class GetEmployeeRoleUnitsByEmployeeRoleIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository) : IRequestHandler<GetEmployeeRoleUnitsByEmployeeRoleIdQuery, IEnumerable<EmployeeRoleUnitVm>>
{
    public async Task<IEnumerable<EmployeeRoleUnitVm>> Handle(GetEmployeeRoleUnitsByEmployeeRoleIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnits = await employeeRoleUnitRepository.GetListByEmployeeRoleId(
            request.EmployeeRoleId,
            cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeRoleUnitVm>>(employeeRoleUnits); 
    }
}