using FlightSearch.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Application.Extensions;
public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<FlightInfo> Merge(
        this IEnumerable<IAsyncEnumerable<FlightInfo>> sources)
    {
        var allFlights = new List<FlightInfo>();

        foreach (var source in sources)
        {
            await foreach (var flight in source)
            {
                allFlights.Add(flight);
            }
        }

        foreach (var flight in allFlights.OrderBy(f => f.Price))
        {
            yield return flight;
        }
    }
}