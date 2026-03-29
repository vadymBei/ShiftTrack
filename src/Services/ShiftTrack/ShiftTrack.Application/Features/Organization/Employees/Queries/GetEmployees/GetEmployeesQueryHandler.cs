using AutoMapper;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployees;

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