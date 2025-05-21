using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateAccount;

public record UpdateAccountCommand(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string PhoneNumber,
    DateTime? DateOfBirth,
    EmployeeGender Gender): IRequest<EmployeeVM>;