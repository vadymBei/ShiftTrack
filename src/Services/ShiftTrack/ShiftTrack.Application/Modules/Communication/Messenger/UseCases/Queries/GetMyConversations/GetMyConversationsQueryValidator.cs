using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMyConversations;

public class GetMyConversationsQueryValidator : AbstractValidator<GetMyConversationsQuery>
{
    public GetMyConversationsQueryValidator()
    {
        RuleFor(x => x.Take).InclusiveBetween(1, MessengerConstants.MaxConversations);
    }
}
