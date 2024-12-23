using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Application.DTOs;
public class FlightDto
{
    public required string FlightNumber { get; init; }
    public required string Departure { get; init; }
    public required string Arrival { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required DateTime DepartureTime { get; init; }
    public required DateTime ArrivalTime { get; init; }
    public required string Provider { get; init; }
}