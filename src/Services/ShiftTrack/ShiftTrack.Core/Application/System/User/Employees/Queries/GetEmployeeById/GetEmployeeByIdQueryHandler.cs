using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<GetEmployeeByIdQuery, EmployeeVM>
{
    public async Task<EmployeeVM> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVM>(employee);
    }
}