using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Structure.Models;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels
{
    [AutoMap(typeof(GroupedDepartmentsByUnit))]
    public class GroupedDepartmentsByUnitVM
    {
        public UnitVM Unit { get; set; }

        public IEnumerable<DepartmentVM> Departments { get; set; }
    }
}
