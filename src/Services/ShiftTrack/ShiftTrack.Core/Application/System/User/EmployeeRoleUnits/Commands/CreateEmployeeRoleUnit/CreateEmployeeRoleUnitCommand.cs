using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public record CreateEmployeeRoleUnitCommand(
    long EmployeeRoleId,
    long UnitId) : IRequest<EmployeeRoleUnitVm>;