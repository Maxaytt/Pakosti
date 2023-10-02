using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.SuperAdministrator.Currencies.Commands;

public static class CreateCurrency
{
    public sealed record Command(string Name) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration)
        {
            RuleFor(c => c.Name).CurrencyName(configuration);
        }
    }

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ICoefficientService _coefficientService;

        public Handler(ICoefficientService coefficientService) =>
            _coefficientService = coefficientService;
        

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var currency = await _coefficientService.Create(request.Name, cancellationToken);
            return new Response(currency);
        }
    }

    public sealed record Response(Currency Currency);
}