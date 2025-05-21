using FluentValidation;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ValidationException = ShiftTrack.Kernel.Exceptions.ValidationException;

namespace ShiftTrack.Kernel.CQRS.Behaviors;

internal abstract class ValidationDecoratorBase<TRequest>(
    IEnumerable<IValidator<TRequest>> validators)
{
    protected async Task ValidateRequest(TRequest request, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return;
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(x => x.ValidateAsync(context, cancellationToken)));

        var validationFailures = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .ToList();

        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }
    }
}

internal sealed class ValidationDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> innerHandler,
    IEnumerable<IValidator<TRequest>> validators)
    : ValidationDecoratorBase<TRequest>(validators),
        IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateRequest(request, cancellationToken);

        return await innerHandler.Handle(request, cancellationToken);
    }
}

internal sealed class ValidationDecorator<TRequest>(
    IRequestHandler<TRequest> innerHandler,
    IEnumerable<IValidator<TRequest>> validators)
    : ValidationDecoratorBase<TRequest>(validators),
        IRequestHandler<TRequest> where TRequest : IRequest
{
    public async Task Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateRequest(request, cancellationToken);

        await innerHandler.Handle(request, cancellationToken);
    }
}