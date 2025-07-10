using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.Models;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(GroupedDepartmentsByUnit))]
public class GroupedDepartmentsByUnitVm
{
    public UnitVm Unit { get; set; }
    public IEnumerable<DepartmentVm> Departments { get; set; }
}