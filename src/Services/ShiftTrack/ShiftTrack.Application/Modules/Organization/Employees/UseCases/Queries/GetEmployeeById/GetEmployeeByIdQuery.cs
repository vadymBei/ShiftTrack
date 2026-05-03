using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(
    long Id) : IRequest<EmployeeVm>;