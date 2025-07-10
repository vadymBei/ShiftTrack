using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public record GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery(
    long EmployeeRoleUnitId) : IRequest<IEnumerable<EmployeeRoleUnitDepartmentVm>>;