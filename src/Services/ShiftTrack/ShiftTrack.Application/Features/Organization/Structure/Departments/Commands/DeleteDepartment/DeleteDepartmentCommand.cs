using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    long Id) : IRequest;