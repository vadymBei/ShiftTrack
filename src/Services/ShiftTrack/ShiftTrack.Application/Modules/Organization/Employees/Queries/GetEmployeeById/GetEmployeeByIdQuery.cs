using ShiftTrack.Application.Modules.Organization.Employees.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(
    long Id) : IRequest<EmployeeVm>;