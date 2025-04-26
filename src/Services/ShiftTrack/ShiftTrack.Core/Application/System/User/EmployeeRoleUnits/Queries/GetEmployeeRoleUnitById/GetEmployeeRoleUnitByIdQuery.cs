using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

public record GetEmployeeRoleUnitByIdQuery(
    long Id) : IRequest<EmployeeRoleUnitVm>;