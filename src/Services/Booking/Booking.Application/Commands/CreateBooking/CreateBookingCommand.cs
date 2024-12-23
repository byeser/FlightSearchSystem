using Booking.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.CreateBooking;
public record CreateBookingCommand : IRequest<BookingResult>
{
    public required string FlightNumber { get; init; }
    public required List<PassengerDto> Passengers { get; init; }
}