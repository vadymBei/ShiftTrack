using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetUserPhoto;

public class GetUserPhotoQueryValidator : AbstractValidator<GetUserPhotoQuery>
{
    public GetUserPhotoQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotNull()
            .WithMessage("EmployeeId is required")
            .GreaterThan(0)
            .WithMessage("EmployeeId must be greater than zero.");
    }
}