using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Models;

namespace ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(GroupedDepartmentsByUnit))]
public class GroupedDepartmentsByUnitVm
{
    public UnitVm Unit { get; set; }
    public IEnumerable<DepartmentVm> Departments { get; set; }
}