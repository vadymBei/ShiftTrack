using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Commands.CreateEmployeeRole;

public record CreateEmployeeRoleCommand(
    EmployeeRoleToCreateDto Dto) : IRequest<EmployeeRoleVm>;