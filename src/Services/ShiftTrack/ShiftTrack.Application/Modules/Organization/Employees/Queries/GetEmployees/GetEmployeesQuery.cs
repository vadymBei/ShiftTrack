using ShiftTrack.Application.Modules.Organization.Employees.Common.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    EmployeesFilterDto Filter) : IRequest<IEnumerable<EmployeeVm>>;