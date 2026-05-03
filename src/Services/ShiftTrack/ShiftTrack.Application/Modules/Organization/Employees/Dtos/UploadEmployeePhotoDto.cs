namespace ShiftTrack.Application.Modules.Organization.Employees.Dtos;

public record UploadEmployeePhotoDto(
    long EmployeeId,
    string PhotoUrl);