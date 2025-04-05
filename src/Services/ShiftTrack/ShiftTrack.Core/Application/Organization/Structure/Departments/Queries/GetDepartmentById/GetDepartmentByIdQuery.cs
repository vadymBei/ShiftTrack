using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(
    long Id) : IRequest<DepartmentVM>;