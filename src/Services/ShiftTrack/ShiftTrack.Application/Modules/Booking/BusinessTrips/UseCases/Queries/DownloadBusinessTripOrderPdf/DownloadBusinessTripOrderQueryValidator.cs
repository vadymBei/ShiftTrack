using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.DownloadBusinessTripOrderPdf;

public class DownloadBusinessTripOrderQueryValidator : AbstractValidator<DownloadBusinessTripOrderQuery>
{
    public DownloadBusinessTripOrderQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}