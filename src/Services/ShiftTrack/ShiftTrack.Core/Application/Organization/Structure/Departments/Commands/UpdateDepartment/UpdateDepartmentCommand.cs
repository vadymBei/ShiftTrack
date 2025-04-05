using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(
    long Id,
    string Name) : IRequest<DepartmentVM>;