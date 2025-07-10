using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

public record GetEmployeeRoleUnitByIdQuery(
    long Id) : IRequest<EmployeeRoleUnitVm>;