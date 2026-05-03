using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;

[AutoMap(typeof(Department))]
public class DepartmentVm
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long? UnitId { get; set; }
    public UnitVm Unit { get; set; }
}