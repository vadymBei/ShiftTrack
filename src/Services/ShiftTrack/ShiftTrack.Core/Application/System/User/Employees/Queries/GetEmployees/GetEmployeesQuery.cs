using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    string SearchPattern,
    long? UnitId,
    long? DepartmentId) : IRequest<IEnumerable<EmployeeVM>>;