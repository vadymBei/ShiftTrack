using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.CreateGroup;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(MessengerConstants.MaxTitleLength);

        RuleFor(x => x.MemberIds)
            .NotEmpty()
            .Must(x => x.Count <= MessengerConstants.MaxGroupMembers)
            .WithMessage("The group has too many members.");
    }
}
