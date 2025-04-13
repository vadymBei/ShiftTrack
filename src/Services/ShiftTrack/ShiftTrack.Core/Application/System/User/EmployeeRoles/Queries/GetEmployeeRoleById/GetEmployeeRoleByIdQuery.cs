using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

public record GetEmployeeRoleByIdQuery(
    long EmployeeRoleId) : IRequest<EmployeeRoleVm>;