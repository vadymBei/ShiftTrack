using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    long Id) : IRequest;