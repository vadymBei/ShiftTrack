using AutoMapper;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(Unit))]
public class UnitVm
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
}