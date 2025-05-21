using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public record GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery(
    long EmployeeRoleUnitId) : IRequest<IEnumerable<EmployeeRoleUnitDepartmentVm>>;