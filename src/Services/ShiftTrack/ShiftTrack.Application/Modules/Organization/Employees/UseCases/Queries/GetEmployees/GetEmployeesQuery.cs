using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployees;

public record GetEmployeesQuery(
    EmployeesFilterDto Filter) : IRequest<IEnumerable<EmployeeVm>>;