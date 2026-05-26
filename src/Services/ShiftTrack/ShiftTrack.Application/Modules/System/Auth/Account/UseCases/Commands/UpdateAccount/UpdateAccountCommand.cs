using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UpdateAccount;

public record UpdateAccountCommand(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string PhoneNumber,
    DateTime? DateOfBirth,
    EmployeeGender Gender): IRequest<EmployeeVm>;