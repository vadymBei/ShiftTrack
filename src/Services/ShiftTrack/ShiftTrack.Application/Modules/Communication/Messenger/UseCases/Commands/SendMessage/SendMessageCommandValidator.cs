using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(MessengerConstants.MaxMessageLength);

        // Text, files or both.
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Text) || x.AttachmentIds is { Count: > 0 })
            .WithName(nameof(SendMessageCommand.Text))
            .WithMessage("The message is empty.");

        RuleFor(x => x.AttachmentIds)
            .Must(x => x is null || x.Count <= MessengerConstants.MaxAttachmentsPerMessage)
            .WithMessage("A message can have at most 10 files.");
    }
}
