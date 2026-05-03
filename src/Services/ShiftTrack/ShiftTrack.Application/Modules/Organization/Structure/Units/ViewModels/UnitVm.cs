using AutoMapper;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;

[AutoMap(typeof(Unit))]
public class UnitVm
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
}