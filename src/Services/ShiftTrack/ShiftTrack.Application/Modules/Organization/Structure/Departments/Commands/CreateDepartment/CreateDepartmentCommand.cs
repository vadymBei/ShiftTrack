using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    long UnitId) : IRequest<DepartmentVm>;