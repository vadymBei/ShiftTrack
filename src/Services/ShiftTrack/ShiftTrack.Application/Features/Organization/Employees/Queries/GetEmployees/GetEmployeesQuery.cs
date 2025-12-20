using ShiftTrack.Application.Features.Organization.Employees.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    EmployeesFilterDto Filter) : IRequest<IEnumerable<EmployeeVm>>;