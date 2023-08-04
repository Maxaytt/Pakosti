using FluentValidation;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class CategoryRules
{
    private const int NameMinLength = 3;
    private const int NameMaxLength = 50;
    public static IRuleBuilder<T, string?> CategoryName<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder
        .MinimumLength(NameMinLength).WithMessage($"Name must contain at least {NameMinLength} characters")
        .MaximumLength(NameMaxLength).WithMessage($"Name must not exceed {NameMaxLength} characters");
}