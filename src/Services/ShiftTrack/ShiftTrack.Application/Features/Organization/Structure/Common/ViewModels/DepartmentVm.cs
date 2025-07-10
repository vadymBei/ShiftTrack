using AutoMapper;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(Department))]
public class DepartmentVm
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long? UnitId { get; set; }
    public UnitVm Unit { get; set; }
}