using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.SuperAdministrator.Currencies.Commands;
using Pakosti.Application.Features.SuperAdministrator.Currencies.Queries;

namespace Pakosti.Api.Controllers;

public class CurrencyController : SuperAdminBaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCurrency.Command request,
        CancellationToken cancellationToken)
    {
        var currency = await Mediator.Send(request, cancellationToken);
        return Created($"api/super/currency/{request.Name}", currency.Currency);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteCurrency.Command request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateCurrency.Command request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{name}")]
    public async Task<ActionResult> Get(string name
        , CancellationToken cancellationToken)
    {
        var request = new GetCurrency.Query(name);
        var response = await Mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var request = new GetCurrencyList.Query();
        var vm = await Mediator.Send(request, cancellationToken);
        return Ok(vm);
    }
    
}