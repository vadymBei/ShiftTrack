using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee
{
    public record CreateEmployeeCommand (
        string Name,
        string Surname,
        string Patronymic,
        string Email,
        string Password,
        string PhoneNumber,
        long? DepartmentId,
        DateTime? DateOfBirth,
        EmployeeGender Gender) : IRequest<EmployeeVM>;
}
