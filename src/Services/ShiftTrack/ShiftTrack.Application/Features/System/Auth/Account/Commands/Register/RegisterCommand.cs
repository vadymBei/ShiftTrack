using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.Register;

public record RegisterCommand(
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string Password,
    string ConfirmPassword,
    string PhoneNumber,
    EmployeeGender Gender) : IRequest<EmployeeVm>;