using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(
    IMapper mapper,
    IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeeByIdQuery, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}