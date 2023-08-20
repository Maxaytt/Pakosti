using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class CurrencyRules
{
    public static IRuleBuilder<T, string?> CurrencyName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var length = configuration.GetSection($"Settings:{nameof(Currency)}:NameLength").Get<int>();
        return ruleBuilder.MinimumLength(length).WithMessage($"Name must consist of {length} letters");
    }
}