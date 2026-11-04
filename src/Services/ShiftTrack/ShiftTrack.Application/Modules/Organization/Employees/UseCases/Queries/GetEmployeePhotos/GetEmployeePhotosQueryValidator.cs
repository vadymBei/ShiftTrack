using FluentValidation;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhotos;

public class GetEmployeePhotosQueryValidator : AbstractValidator<GetEmployeePhotosQuery>
{
    public GetEmployeePhotosQueryValidator()
    {
        RuleFor(x => x.EmployeeIds)
            .NotNull()
            .Must(x => x.Count <= EmployeePhotoConstants.MaxEmployeesPerRequest)
            .WithMessage(EmployeeExceptionsLocalization.TOO_MANY_EMPLOYEES);
    }
}
