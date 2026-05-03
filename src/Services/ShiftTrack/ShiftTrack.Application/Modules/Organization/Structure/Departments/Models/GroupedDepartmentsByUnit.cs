using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Models;

public class GroupedDepartmentsByUnit
{
    public Unit Unit { get; set; }
    public IEnumerable<Department> Departments { get; set; }
}