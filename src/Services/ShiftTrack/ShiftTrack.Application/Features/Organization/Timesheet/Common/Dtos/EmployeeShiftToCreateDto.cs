namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;

public class EmployeeShiftToCreateDto
{
    public long EmployeeId { get; set; }
    public long ShiftId { get; set; }
    public DateTime Date { get; set; }
}