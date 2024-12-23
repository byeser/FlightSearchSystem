using Microsoft.AspNetCore.Mvc;
using MediatR;
using FlightSearch.Application.Queries.SearchFlights;
using FlightSearch.Shared.Models;
using FlightSearch.Api.Metrics;
using Prometheus;

namespace FlightSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly IMediator _mediator; 

    public FlightController(IMediator mediator)
    {
        _mediator = mediator;
    }

 
    [HttpPost("search")]
    [ProducesResponseType(typeof(IAsyncEnumerable<FlightInfo>), 200)] 
    public async Task<ActionResult<IAsyncEnumerable<FlightInfo>>> SearchFlights(
        [FromBody] SearchFlightsQuery query,
        CancellationToken cancellationToken)
    {
        using var timer = FlightSearchMetrics.SearchDuration.NewTimer();
        FlightSearchMetrics.SearchRequests.Inc();

        var results = await _mediator.Send(query, cancellationToken);
        return Ok(results);
    }
}