using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.Register;

public record RegisterCommand(
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string Password,
    string ConfirmPassword,
    string PhoneNumber,
    EmployeeGender Gender) : IRequest<EmployeeVm>;