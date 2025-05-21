using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public record CreateEmployeeRoleCommand(
    EmployeeRoleToCreateDto Dto) : IRequest<EmployeeRoleVm>;