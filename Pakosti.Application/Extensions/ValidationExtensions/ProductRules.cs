using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class ProductRules
{
    public static IRuleBuilder<T, string?> ProductName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection($"Settings:{nameof(Product)}:NameLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        return ruleBuilder
            .MinimumLength(minLength)
            .WithMessage($"Name must contain at least {minLength} characters")
            .MaximumLength(maxLength)
            .WithMessage($"Name must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, string?> ProductDescription<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection($"Settings:{nameof(Product)}:DescriptionLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        return ruleBuilder
            .MinimumLength(minLength)
            .WithMessage($"Description must contain at least {minLength} characters")
            .MaximumLength(maxLength)
            .WithMessage($"Description must not exceed {maxLength} characters");
    }
}