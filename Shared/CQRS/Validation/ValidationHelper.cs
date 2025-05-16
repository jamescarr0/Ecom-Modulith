using FluentValidation;

namespace Shared.CQRS.Validation;

public static class ValidationHelper
{
    public static async Task ValidateCommand<TCommand>(TCommand command, IValidator<TCommand> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        if (errors.Count != 0)
        {
            var errorMessage = string.Join("\n", errors);
            throw new ValidationException($"Validation Failed [{nameof(validator.GetType)} - ]:\n{errorMessage}");
        }
    }
}
