using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

[AutoMap(typeof(Position))]
public class PositionVM
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}