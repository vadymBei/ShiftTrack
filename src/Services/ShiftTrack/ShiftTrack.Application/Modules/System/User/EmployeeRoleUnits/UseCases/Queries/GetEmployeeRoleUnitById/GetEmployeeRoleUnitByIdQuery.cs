using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitById;

public record GetEmployeeRoleUnitByIdQuery(
    long Id) : IRequest<EmployeeRoleUnitVm>;