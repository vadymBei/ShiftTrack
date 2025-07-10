using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.Models;

public class GroupedDepartmentsByUnit
{
    public Unit Unit { get; set; }
    public IEnumerable<Department> Departments { get; set; }
}