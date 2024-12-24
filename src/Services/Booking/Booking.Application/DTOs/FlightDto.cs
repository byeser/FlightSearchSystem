using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.DTOs;
public record FlightDto
{
    public Guid Id { get; init; }
    public required string FlightNumber { get; init; }
    public required string Departure { get; init; }
    public required string Arrival { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
    public required string Provider { get; init; }  // Provider ekledik
    public required DateTime DepartureTime { get; init; }
    public required DateTime ArrivalTime { get; init; }
}