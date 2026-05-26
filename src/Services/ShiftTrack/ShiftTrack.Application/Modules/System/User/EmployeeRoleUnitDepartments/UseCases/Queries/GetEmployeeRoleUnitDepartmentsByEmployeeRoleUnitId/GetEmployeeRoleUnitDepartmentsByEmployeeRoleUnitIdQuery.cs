using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public record GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery(
    long EmployeeRoleUnitId) : IRequest<IEnumerable<EmployeeRoleUnitDepartmentVm>>;