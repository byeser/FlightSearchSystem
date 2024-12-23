using Booking.Application.Commands.CancelBooking;
using Booking.Application.Commands.CreateBooking;
using Booking.Application.DTOs;
using Booking.Application.Queries.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponse>> Get(Guid id)
    {
        var query = new GetBookingByIdQuery(id);
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var command = new CancelBookingCommand(id);
        await _mediator.Send(command);
        return Ok();
    }
}