using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.AddGroupMembers;

public class AddGroupMembersCommandValidator : AbstractValidator<AddGroupMembersCommand>
{
    public AddGroupMembersCommandValidator()
    {
        RuleFor(x => x.EmployeeIds)
            .NotEmpty()
            .Must(x => x.Count <= MessengerConstants.MaxGroupMembers)
            .WithMessage("The group has too many members.");
    }
}
