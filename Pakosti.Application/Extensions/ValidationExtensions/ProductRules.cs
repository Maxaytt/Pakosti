using FluentValidation;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class ProductRules
{
    private const int NameMinLength = 5;
    private const int NameMaxLength = 150;
    private const int DescriptionMinLength = 20;
    private const int DescriptionMaxLength = 1500;
    
    public static IRuleBuilder<T, string?> ProductName<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder
        .MinimumLength(NameMinLength)
        .WithMessage($"Name must contain at least {NameMinLength} characters")
        .MaximumLength(NameMaxLength)
        .WithMessage($"Name must not exceed {NameMaxLength} characters");
    
    public static IRuleBuilder<T, string?> ProductDescription<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder
        .MinimumLength(DescriptionMinLength)
        .WithMessage($"Description must contain at least {DescriptionMinLength} characters")
        .MaximumLength(DescriptionMaxLength)
        .WithMessage($"Description must not exceed {DescriptionMaxLength} characters");
}