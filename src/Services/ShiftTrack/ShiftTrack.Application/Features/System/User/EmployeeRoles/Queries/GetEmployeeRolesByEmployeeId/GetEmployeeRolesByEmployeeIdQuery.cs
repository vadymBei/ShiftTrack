using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

public record GetEmployeeRolesByEmployeeIdQuery(
    long EmployeeId) : IRequest<IEnumerable<EmployeeRoleVm>>;