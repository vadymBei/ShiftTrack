using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitById;

public class GetEmployeeRoleUnitByIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository) : IRequestHandler<GetEmployeeRoleUnitByIdQuery, EmployeeRoleUnitVm>
{
    public async Task<EmployeeRoleUnitVm> Handle(GetEmployeeRoleUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await employeeRoleUnitRepository.GetById(
            request.Id,
            cancellationToken);
        
        return mapper.Map<EmployeeRoleUnitVm>(employeeRoleUnit);
    }
}