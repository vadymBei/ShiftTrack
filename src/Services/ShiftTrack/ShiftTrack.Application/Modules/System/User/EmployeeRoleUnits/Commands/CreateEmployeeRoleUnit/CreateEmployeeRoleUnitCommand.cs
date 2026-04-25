using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public record CreateEmployeeRoleUnitCommand(
    long EmployeeRoleId,
    long UnitId) : IRequest<EmployeeRoleUnitVm>;