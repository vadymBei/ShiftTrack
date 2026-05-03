using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Models;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;

[AutoMap(typeof(GroupedDepartmentsByUnit))]
public class GroupedDepartmentsByUnitVm
{
    public UnitVm Unit { get; set; }
    public IEnumerable<DepartmentVm> Departments { get; set; }
}