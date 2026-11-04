using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IMessageService
{
    Task<IReadOnlyList<Message>> GetPage(long conversationId, long? beforeId, int take, CancellationToken cancellationToken);
    /// <summary>Text, files uploaded before (<paramref name="attachmentIds"/>) or both.</summary>
    Task<Message> Send(long conversationId, string text, Guid? clientMessageId, long? replyToMessageId,
        IReadOnlyCollection<long> attachmentIds, CancellationToken cancellationToken);
    Task<Message> Edit(long messageId, string text, CancellationToken cancellationToken);
    Task<Message> Delete(long messageId, CancellationToken cancellationToken);
}
