using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Services;

/// <summary>
/// Files on the disk of the API: "wwwroot/uploads/messenger/{year}/{month}/{random}.ext", or "Messenger:AttachmentsPath".
/// They aren't served as static files (see Program.cs): only the attachment endpoint, for the conversation members.
/// With several API instances or containers the folder has to be shared (a volume).
/// </summary>
public class LocalAttachmentStorage(
    IHostEnvironment hostEnvironment,
    IConfiguration configuration,
    ILogger<LocalAttachmentStorage> logger) : IAttachmentStorage
{
    public const string RequestPath = "/uploads/messenger";

    private string Root => configuration["Messenger:AttachmentsPath"] is { Length: > 0 } path
        ? path
        : Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "uploads", "messenger");

    public async Task<string> Save(Stream content, string extension, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var storedName = $"{now:yyyy}/{now:MM}/{Guid.NewGuid():N}{extension}";
        var path = FullPath(storedName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            await using var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, true);
            await content.CopyToAsync(file, cancellationToken);
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or IOException)
        {
            // Usually the folder (a docker volume) isn't writable for the user the API runs as.
            logger.LogError("{LogName} Can't write the file {Path}: {Error}", "SaveAttachment", path, exception.Message);
            Delete(storedName);

            throw new MessengerException(
                MessengerExceptionsLocalization.ATTACHMENT_STORAGE_UNAVAILABLE,
                nameof(MessengerExceptionsLocalization.ATTACHMENT_STORAGE_UNAVAILABLE),
                HttpStatusCode.InternalServerError);
        }
        catch
        {
            Delete(storedName);
            throw;
        }

        return storedName;
    }

    public Stream Open(string storedName)
    {
        var path = FullPath(storedName);

        return File.Exists(path)
            ? new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true)
            : null;
    }

    public void Delete(string storedName)
    {
        try
        {
            var path = FullPath(storedName);

            if (File.Exists(path))
                File.Delete(path);
        }
        catch (Exception exception)
        {
            logger.LogWarning("{LogName} Failed to delete file {StoredName}: {Error}",
                "DeleteAttachment", storedName, exception.Message);
        }
    }

    /// <summary>Stored names are made here, but a name must never lead out of the folder anyway.</summary>
    private string FullPath(string storedName)
    {
        var root = Path.GetFullPath(Root);
        var path = Path.GetFullPath(Path.Combine(root, storedName));

        if (!path.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new InvalidOperationException($"The attachment name {storedName} leads out of the storage.");

        return path;
    }
}
