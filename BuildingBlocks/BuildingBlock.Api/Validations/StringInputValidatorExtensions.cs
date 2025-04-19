using FluentValidation;

namespace BuildingBlock.Api.Validations;

public static class StringInputValidatorExtensions
{
    public static IRuleBuilderOptions<T, string?> StringInput<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int maxLength,
        bool isRequired = true)
    {
        if (isRequired)
        {
            ruleBuilder = ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
        }

        return ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage("{PropertyName} must not exceed {MaxLength} characters.");
    }
}
