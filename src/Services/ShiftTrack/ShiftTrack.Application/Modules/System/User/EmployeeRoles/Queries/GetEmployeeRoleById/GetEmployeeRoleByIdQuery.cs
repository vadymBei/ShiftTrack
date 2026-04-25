using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

public record GetEmployeeRoleByIdQuery(
    long EmployeeRoleId) : IRequest<EmployeeRoleVm>;