using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById
{
    public record GetEmployeeByIdQuery(
        long Id) : IRequest<EmployeeVM>;
}
