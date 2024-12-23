using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Shared.Models;

public record FlightSearchRequest
{
    public required string Origin { get; init; }
    public required string Destination { get; init; }
    public required DateTime DepartureDate { get; init; }
    public DateTime? ReturnDate { get; init; }
    public required int PassengerCount { get; init; }
}
