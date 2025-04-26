using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public record GetEmployeeRoleUnitsByEmployeeRoleIdQuery(
    long EmployeeRoleId) : IRequest<IEnumerable<EmployeeRoleUnitVm>>;