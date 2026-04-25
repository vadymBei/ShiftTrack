using ShiftTrack.Application.Modules.System.User.Common.Dtos;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public record CreateEmployeeRoleCommand(
    EmployeeRoleToCreateDto Dto) : IRequest<EmployeeRoleVm>;