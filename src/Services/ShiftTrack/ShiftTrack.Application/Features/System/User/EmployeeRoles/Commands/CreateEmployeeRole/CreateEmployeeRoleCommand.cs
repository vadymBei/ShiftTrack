using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.CreateEmployeeRole;

public record CreateEmployeeRoleCommand(
    EmployeeRoleToCreateDto Dto) : IRequest<EmployeeRoleVm>;