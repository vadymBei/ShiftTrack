using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.UpdateAccount;

public record UpdateAccountCommand(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string PhoneNumber,
    DateTime? DateOfBirth,
    EmployeeGender Gender): IRequest<EmployeeVm>;