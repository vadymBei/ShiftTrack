using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(
    long Id,
    string Name) : IRequest<DepartmentVm>;