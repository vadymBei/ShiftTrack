using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(
    long Id) : IRequest<EmployeeVM>;