using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

public class AttachmentCleanup(
    IMessageAttachmentRepository attachmentRepository,
    IAttachmentStorage attachmentStorage,
    ILogger<AttachmentCleanup> logger) : IAttachmentCleanup
{
    private const int Batch = 200;

    public async Task<int> RemoveUnsent(CancellationToken cancellationToken)
    {
        var before = DateTime.UtcNow - MessengerConstants.UnsentAttachmentLifetime;
        var removed = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var unsent = await attachmentRepository.GetUnsentBefore(before, Batch, cancellationToken);

            if (unsent.Count == 0)
                break;

            await attachmentRepository.Remove(unsent, cancellationToken);

            foreach (var attachment in unsent)
                attachmentStorage.Delete(attachment.StoredName);

            removed += unsent.Count;

            if (unsent.Count < Batch)
                break;
        }

        if (removed > 0)
            logger.LogInformation("{LogName} Removed {Count} files that were uploaded but never sent",
                "RemoveUnsentAttachments", removed);

        return removed;
    }
}
