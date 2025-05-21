using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public record GetEmployeeRoleUnitsByEmployeeRoleIdQuery(
    long EmployeeRoleId) : IRequest<IEnumerable<EmployeeRoleUnitVm>>;