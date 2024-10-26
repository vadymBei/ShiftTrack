using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Domain.Organization.Structure.Models
{
    public class GroupedDepartmentsByUnit
    {
        public Unit Unit { get; set; }

        public IEnumerable<Department> Departments { get; set; }
    }
}
