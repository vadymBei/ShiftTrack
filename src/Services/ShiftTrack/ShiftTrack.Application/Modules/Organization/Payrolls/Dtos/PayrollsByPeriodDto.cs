namespace ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;

public record PayrollsByPeriodDto(
    DateTime Period,
    HashSet<long> EmployeeIds);