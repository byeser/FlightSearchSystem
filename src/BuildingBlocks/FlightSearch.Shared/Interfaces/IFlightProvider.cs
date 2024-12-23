using FlightSearch.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Shared.Interfaces;
public interface IFlightProvider
{
    string ProviderName { get; }
    IAsyncEnumerable<FlightInfo> GetFlightsAsync(
        FlightSearchRequest request,
        CancellationToken cancellationToken = default);
}