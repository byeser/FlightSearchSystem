using MediatR;
using Microsoft.Extensions.Logging;
using FlightSearch.Shared.Models;
using FlightSearch.Shared.Interfaces;
using FlightSearch.Application.Extensions;

namespace FlightSearch.Application.Queries.SearchFlights;

public class SearchFlightsQueryHandler
    : IRequestHandler<SearchFlightsQuery, IAsyncEnumerable<FlightInfo>>
{
    private readonly IEnumerable<IFlightProvider> _providers;
    private readonly ILogger<SearchFlightsQueryHandler> _logger;

    public SearchFlightsQueryHandler(
        IEnumerable<IFlightProvider> providers,
        ILogger<SearchFlightsQueryHandler> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    public async Task<IAsyncEnumerable<FlightInfo>> Handle(
        SearchFlightsQuery request,
        CancellationToken cancellationToken)
    {
        var searchTasks = _providers.Select(provider =>
            provider.GetFlightsAsync(
                new FlightSearchRequest
                {
                    Origin = request.Origin,
                    Destination = request.Destination,
                    DepartureDate = request.DepartureDate,
                    ReturnDate = request.ReturnDate,
                    PassengerCount = request.PassengerCount
                },
                cancellationToken));

        return searchTasks.Merge();
    }
}