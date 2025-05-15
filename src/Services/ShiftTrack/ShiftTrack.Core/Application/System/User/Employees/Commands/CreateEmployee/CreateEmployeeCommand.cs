using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand (
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string Password,
    string ConfirmPassword,
    string PhoneNumber,
    EmployeeGender Gender) : IRequest<EmployeeVM>;