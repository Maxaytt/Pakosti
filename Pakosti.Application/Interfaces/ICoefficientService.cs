using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface ICoefficientService
{
    public Task<Currency> Create(string name, CancellationToken cancellationToken);
    public Task UpdateCurrencyOnChanges(CancellationToken cancellationToken);
}