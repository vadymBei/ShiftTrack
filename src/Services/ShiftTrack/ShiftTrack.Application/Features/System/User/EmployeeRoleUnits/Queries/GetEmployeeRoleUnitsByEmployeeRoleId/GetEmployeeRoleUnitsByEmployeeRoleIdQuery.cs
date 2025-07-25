using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public record GetEmployeeRoleUnitsByEmployeeRoleIdQuery(
    long EmployeeRoleId) : IRequest<IEnumerable<EmployeeRoleUnitVm>>;