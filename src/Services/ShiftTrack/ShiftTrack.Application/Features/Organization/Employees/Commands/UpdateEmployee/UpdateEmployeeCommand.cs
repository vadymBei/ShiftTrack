using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    long? DepartmentId,
    long? PositionId,
    DateTime? DateOfBirth,
    EmployeeGender Gender) : IRequest<EmployeeVm>;