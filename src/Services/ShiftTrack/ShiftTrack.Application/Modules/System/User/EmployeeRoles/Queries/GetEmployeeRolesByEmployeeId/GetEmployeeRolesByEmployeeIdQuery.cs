using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

public record GetEmployeeRolesByEmployeeIdQuery(
    long EmployeeId) : IRequest<IEnumerable<EmployeeRoleVm>>;