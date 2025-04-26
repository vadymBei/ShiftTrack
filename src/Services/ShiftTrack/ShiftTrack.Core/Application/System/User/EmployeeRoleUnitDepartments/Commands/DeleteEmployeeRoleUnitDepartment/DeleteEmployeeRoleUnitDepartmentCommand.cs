using MediatR;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public record DeleteEmployeeRoleUnitDepartmentCommand(
    long Id) : IRequest;