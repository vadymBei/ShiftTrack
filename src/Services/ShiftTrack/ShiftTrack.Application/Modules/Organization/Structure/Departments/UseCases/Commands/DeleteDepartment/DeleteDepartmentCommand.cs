using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    long Id) : IRequest;