using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public record GetEmployeeRoleUnitsByEmployeeRoleIdQuery(
    long EmployeeRoleId) : IRequest<IEnumerable<EmployeeRoleUnitVm>>;