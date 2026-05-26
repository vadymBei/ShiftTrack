using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(
    DepartmentToUpdateDto Data) : IRequest<DepartmentVm>;