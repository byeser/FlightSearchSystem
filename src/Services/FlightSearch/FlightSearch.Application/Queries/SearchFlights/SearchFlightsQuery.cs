using FlightSearch.Application.DTOs;
using FlightSearch.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Application.Queries.SearchFlights;
public record SearchFlightsQuery : IRequest<IAsyncEnumerable<FlightInfo>>
{
    public required string Origin { get; init; }
    public required string Destination { get; init; }
    public required DateTime DepartureDate { get; init; }
    public DateTime? ReturnDate { get; init; }
    public required int PassengerCount { get; init; }
}