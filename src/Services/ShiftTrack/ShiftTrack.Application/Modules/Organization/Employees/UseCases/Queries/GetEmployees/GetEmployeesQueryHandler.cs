using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployees;

public class GetEmployeesQueryHandler(
    IMapper mapper,
    IEmployeeService employeeService)
    : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeVm>>
{
    public async Task<IEnumerable<EmployeeVm>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await employeeService.GetEmployees(
            request.Filter,
            cancellationToken);

        return mapper.Map<List<EmployeeVm>>(employees);
    }
}