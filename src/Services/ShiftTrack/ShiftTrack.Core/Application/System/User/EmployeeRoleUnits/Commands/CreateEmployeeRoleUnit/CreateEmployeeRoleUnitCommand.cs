using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public record CreateEmployeeRoleUnitCommand(
    long EmployeeRoleId,
    long UnitId) : IRequest<EmployeeRoleUnitVm>;