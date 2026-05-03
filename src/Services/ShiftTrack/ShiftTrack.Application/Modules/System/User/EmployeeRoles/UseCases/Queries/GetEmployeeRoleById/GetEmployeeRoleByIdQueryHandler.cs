using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Queries.GetEmployeeRoleById;

public class GetEmployeeRoleByIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleRepository employeeRoleRepository) : IRequestHandler<GetEmployeeRoleByIdQuery, EmployeeRoleVm>
{
    public async Task<EmployeeRoleVm> Handle(GetEmployeeRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRole = await employeeRoleRepository.GetById(
            request.EmployeeRoleId,
            cancellationToken);
        
        return mapper.Map<EmployeeRoleVm>(employeeRole);
    }
}