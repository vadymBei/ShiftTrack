namespace ShiftTrack.Application.Modules.Communication.Messenger.Dtos;

public record SendMessageDto(
    string Text,
    Guid? ClientMessageId,
    long? ReplyToMessageId);
