using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public record CreateEmployeeRoleUnitCommand(
    long EmployeeRoleId,
    long UnitId) : IRequest<EmployeeRoleUnitVm>;