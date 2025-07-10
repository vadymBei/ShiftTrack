using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(
    long Id) : IRequest<EmployeeVm>;