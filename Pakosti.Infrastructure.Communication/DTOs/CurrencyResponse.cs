// ReSharper disable InconsistentNaming

using System.Numerics;

namespace Pakosti.Infrastructure.Communication.DTOs;

public class CurrencyResponse
{
    public Dictionary<string, decimal> Rates { get; set; } = null!;
}