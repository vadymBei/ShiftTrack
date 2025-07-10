using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    string SearchPattern,
    long? UnitId,
    long? DepartmentId) : IRequest<IEnumerable<EmployeeVm>>;