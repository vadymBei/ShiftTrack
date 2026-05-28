namespace ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;

public record MarkPayrollAsPaidDto(
    long EmployeeId,
    DateTime Period);