using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById
{
    public class GetShiftByIdQueryValidator : AbstractValidator<GetShiftByIdQuery>
    {
        public GetShiftByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                    .WithMessage("Id is required")
                .Must(x => x > 0)
                    .WithMessage("Id must be bigger than 0");
        }
    }
}
