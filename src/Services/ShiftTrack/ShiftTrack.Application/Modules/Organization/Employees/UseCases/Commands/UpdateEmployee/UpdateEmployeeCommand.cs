using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    long? DepartmentId,
    long? PositionId,
    DateTime? DateOfBirth,
    EmployeeGender Gender) : IRequest<EmployeeVm>;