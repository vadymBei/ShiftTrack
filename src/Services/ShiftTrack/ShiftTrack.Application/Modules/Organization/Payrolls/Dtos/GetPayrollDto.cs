namespace ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;

public record GetPayrollDto(
    long EmployeeId,
    DateTime Period);