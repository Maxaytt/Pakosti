using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Communication.DTOs;

namespace Pakosti.Infrastructure.Communication.Services;

public class CoefficientService : ICoefficientService
{
    private readonly HttpClient _client;
    private readonly IPakostiDbContext _context;
    private readonly string _appId;
    private readonly string _baseUrl;

    private const decimal MinimalChanges = 0.05M;

    public CoefficientService(IPakostiDbContext context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _client = clientFactory.CreateClient("CurrencyApiClient");
        _appId = Environment.GetEnvironmentVariable("CURRENCY_APP_ID")!; 
        _baseUrl = Environment.GetEnvironmentVariable("CURRENCY_APP_BASE_URL")!;
    }

    public async Task<Currency> Create(string name, CancellationToken cancellationToken)
    {
        if (await _context.Currencies.AnyAsync(c => c.Name == name, cancellationToken))
            throw new ConflictException(nameof(Currency), name);
        
        var fullUrl = string.Concat(_baseUrl, $"?app_id={_appId}&symbols={name}");
        var response = await _client.GetAsync(fullUrl, cancellationToken);
        var data = await response.Content.ReadFromJsonAsync<CurrencyResponse>(cancellationToken: cancellationToken);
        
        if (!data!.Rates.Any())
            throw new NotFoundException(nameof(Currency), name);

        var currency = new Currency { Coefficient = data.Rates[name], Name = name };
        await _context.Currencies.AddAsync(currency, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return currency;
    }
    
    public async Task UpdateCurrencyOnChanges(CancellationToken cancellationToken)
    {
        var symbols = GetCurrencySymbols(_context);
        var fullUrl = string.Concat(_baseUrl, $"?app_id={_appId}&symbols={symbols}");
        
        var response = await _client.GetAsync(fullUrl, cancellationToken);
        var data = await response.Content.ReadFromJsonAsync<CurrencyResponse>(cancellationToken: cancellationToken);
        
        foreach (var (key, value) in data!.Rates)
        {
            var currency = _context.Currencies.SingleOrDefault(c => c.Name == key);
            if (currency == null) continue;
            var dbRate = currency.Coefficient;
            var changePercent = Math.Abs((value - dbRate) / dbRate * 100);

            if (changePercent > MinimalChanges * 100)
            {
                currency.Coefficient = value;
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    
    
    private static string GetCurrencySymbols(IPakostiDbContext context)
    {
        var currencyNames = context.Currencies.Select(c => c.Name).ToList();
        return string.Join(",", currencyNames);
    }
}