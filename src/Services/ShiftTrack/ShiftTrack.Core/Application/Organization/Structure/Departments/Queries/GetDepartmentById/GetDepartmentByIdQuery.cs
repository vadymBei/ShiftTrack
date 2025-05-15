using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(
    long Id) : IRequest<DepartmentVM>;