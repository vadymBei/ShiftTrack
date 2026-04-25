using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public record DeleteEmployeeRoleUnitDepartmentCommand(
    long Id) : IRequest;