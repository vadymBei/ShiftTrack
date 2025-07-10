using AutoMapper;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(Position))]
public class PositionVm
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}