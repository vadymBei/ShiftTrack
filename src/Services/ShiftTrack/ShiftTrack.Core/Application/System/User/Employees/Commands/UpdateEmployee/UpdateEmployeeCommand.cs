using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand(
        long Id,
        string Name,
        string Surname,
        string Patronymic,
        string Email,
        string PhoneNumber,
        long DepartmentId,
        long PositionId,
        DateTime? DateOfBirth,
        EmployeeGender Gender) : IRequest<EmployeeVM>;
}
