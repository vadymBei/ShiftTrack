using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

public record GetEmployeeRoleUnitByIdQuery(
    long Id) : IRequest<EmployeeRoleUnitVm>;