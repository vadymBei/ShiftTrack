using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

public record GetEmployeeRolesByEmployeeIdQuery(
    long EmployeeId) : IRequest<IEnumerable<EmployeeRoleVm>>;