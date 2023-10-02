using MediatR;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.SuperAdministrator.Currencies.Commands;

public static class UpdateCurrency
{
    public sealed record Command() : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly ICoefficientService _coefficientService;

        public Handler(ICoefficientService coefficientService) =>
            _coefficientService = coefficientService;

        public async Task Handle(Command request, CancellationToken cancellationToken) =>
            await _coefficientService.UpdateCurrencyOnChanges(cancellationToken);
    }
}