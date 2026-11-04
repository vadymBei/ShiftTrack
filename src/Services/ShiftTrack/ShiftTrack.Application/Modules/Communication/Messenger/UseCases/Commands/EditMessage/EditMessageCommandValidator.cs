using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.EditMessage;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("The message is empty.")
            .MaximumLength(MessengerConstants.MaxMessageLength);
    }
}
