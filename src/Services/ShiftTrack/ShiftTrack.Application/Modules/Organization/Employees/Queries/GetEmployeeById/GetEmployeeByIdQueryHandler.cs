using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<GetEmployeeByIdQuery, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}