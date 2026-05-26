using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Commands.CreateEmployeeRoleUnitDepartment;

public record CreateEmployeeRoleUnitDepartmentCommand(
    long DepartmentId,
    long EmployeeRoleUnitId) : IRequest<EmployeeRoleUnitDepartmentVm>;