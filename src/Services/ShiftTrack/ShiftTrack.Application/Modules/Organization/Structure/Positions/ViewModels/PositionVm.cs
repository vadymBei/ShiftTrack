using AutoMapper;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;

[AutoMap(typeof(Position))]
public class PositionVm
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal HourlyRate { get; set; }
}