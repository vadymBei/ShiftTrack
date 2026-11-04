using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessages;

public class GetMessagesQueryValidator : AbstractValidator<GetMessagesQuery>
{
    public GetMessagesQueryValidator()
    {
        RuleFor(x => x.Take).InclusiveBetween(1, MessengerConstants.MaxMessagesPage);
    }
}
