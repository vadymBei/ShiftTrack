using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

public record GetEmployeeRoleByIdQuery(
    long EmployeeRoleId) : IRequest<EmployeeRoleVm>;