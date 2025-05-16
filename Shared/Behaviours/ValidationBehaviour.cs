using FluentValidation;
using MediatR;
using Shared.CQRS.Command;

namespace Shared.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);

        // Iterate all the validators and aggregate the results
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

        var validationErrors = validationResults
            .Where(e => e.Errors.Any())
            .SelectMany(e => e.Errors)
            .ToList();

        if (validationErrors.Any())
        {
            throw new ValidationException(validationErrors);
        }

        return await next(cancellationToken);
    }
}
