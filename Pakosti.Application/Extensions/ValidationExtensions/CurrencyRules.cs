using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class CurrencyRules
{
    public static IRuleBuilder<T, string?> CurrencyName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var length = configuration.GetValue<int>($"Settings:{nameof(Currency)}:Length");
        return ruleBuilder.MinimumLength(length).WithMessage($"Name must consist of {length} letters");
    }
}