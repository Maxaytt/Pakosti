using FluentValidation;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class ReviewRules
{
    private const int HeaderMinLength = 5;
    private const int HeaderMaxLength = 100;
    private const int BodyMinLength = 25;
    private const int BodyMaxLength = 1500;
    public static IRuleBuilder<T, string?> ReviewHeader<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder
        .MinimumLength(HeaderMinLength).WithMessage($"Header must contain at least {HeaderMinLength} characters")
        .MaximumLength(HeaderMaxLength).WithMessage($"Header must not exceed {HeaderMaxLength} characters");
    
    public static IRuleBuilder<T, string?> ReviewBody<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder
        .MinimumLength(BodyMinLength).WithMessage($"Body must contain at least {BodyMinLength} characters")
        .MaximumLength(BodyMaxLength).WithMessage($"Body must not exceed {BodyMaxLength} characters");
}