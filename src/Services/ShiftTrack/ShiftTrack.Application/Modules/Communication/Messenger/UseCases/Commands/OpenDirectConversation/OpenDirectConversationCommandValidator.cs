using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.OpenDirectConversation;

public class OpenDirectConversationCommandValidator : AbstractValidator<OpenDirectConversationCommand>
{
    public OpenDirectConversationCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
    }
}
