using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Commands.CreateEmployeeRoleUnitDepartment;

public record CreateEmployeeRoleUnitDepartmentCommand(
    long DepartmentId,
    long EmployeeRoleUnitId) : IRequest<EmployeeRoleUnitDepartmentVm>;