namespace ShiftTrack.Application.Modules.Organization.Employees.Interfaces;

/// <summary>Where the photos of the employees are kept.</summary>
public interface IEmployeePhotoStorage
{
    /// <summary>Saves the content under a new random name with the extension and returns the name.</summary>
    Task<string> Save(Stream content, string extension, CancellationToken cancellationToken);

    /// <summary>Null if the file is gone.</summary>
    Stream Open(string storedName);

    /// <summary>Does nothing if the file is already gone; failures are logged, not thrown.</summary>
    void Delete(string storedName);
}
