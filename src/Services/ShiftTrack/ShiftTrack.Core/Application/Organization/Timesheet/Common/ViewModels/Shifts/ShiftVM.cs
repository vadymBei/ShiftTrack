using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

[AutoMap(typeof(Shift))]
public class ShiftVM
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Dercription { get; set; }
    public string Color { get; set; }
    public ShiftType Type { get; set; }
}