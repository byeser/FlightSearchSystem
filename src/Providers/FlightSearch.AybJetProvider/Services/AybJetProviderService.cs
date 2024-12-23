using FlightSearch.AybJetProvider.Models;
using FlightSearch.Domain.Entities;
using FlightSearch.Domain.ValueObjects;
using FlightSearch.Shared.Http;
using FlightSearch.Shared.Interfaces;
using FlightSearch.Shared.Models;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace FlightSearch.AybJetProvider.Services;
public class AybJetProviderService : IFlightProvider
{
    private readonly IGenericHttpClient _httpClient;
    private readonly ILogger<AybJetProviderService> _logger;

    public string ProviderName => "AybJet";

    public async IAsyncEnumerable<FlightInfo> GetFlightsAsync(
        FlightSearchRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {

        var jsonContent = await File.ReadAllTextAsync("AybJet-Provider-Response.json", cancellationToken);
        var flights = JsonConvert.DeserializeObject<List<AybJetFlightResponse>>(jsonContent);

        if (flights == null) yield break;

        flights = flights.Where(x => x.DepartureTime.Date == request.DepartureDate.Date).ToList();

        foreach (var flight in flights)
        {
            if (cancellationToken.IsCancellationRequested) yield break;

            yield return new FlightInfo
            {
                FlightNumber = flight.FlightNumber,
                Departure = flight.Departure,
                Arrival = flight.Arrival,
                Price = flight.Price,
                Currency = flight.Currency,
                Duration = flight.Duration,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                ProviderName = ProviderName
            };
        }
    }
}