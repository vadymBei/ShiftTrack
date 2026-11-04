namespace ShiftTrack.Application.Modules.Organization.Employees.Models;

/// <summary>A photo as it comes from the request.</summary>
public record PhotoUpload(
    Stream Content,
    string FileName,
    long Length);
