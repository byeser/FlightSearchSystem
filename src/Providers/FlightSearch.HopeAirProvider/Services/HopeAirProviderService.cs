
using System.Xml.Linq;
using FlightSearch.Shared.Interfaces;
using FlightSearch.Shared.Models;
using FlightSearch.Shared.Http;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace FlightSearch.HopeAirProvider.Services;

public class HopeAirProviderService : IFlightProvider
{
    private readonly IGenericHttpClient _httpClient;
    private readonly ILogger<HopeAirProviderService> _logger;

    public string ProviderName => "HopeAir";

    public HopeAirProviderService(
        IGenericHttpClient httpClient,
        ILogger<HopeAirProviderService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    public async IAsyncEnumerable<FlightInfo> GetFlightsAsync(
    FlightSearchRequest request,
    [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var xmlContent = await File.ReadAllTextAsync("HopeAir-Provider-Response.xml", cancellationToken);
        var doc = XDocument.Parse(xmlContent);

        XNamespace sky = "http://skyblue.com/flight";
               
        var flights = doc.Descendants(sky + "flight")
            .Select(f => new FlightInfo
            {
                FlightNumber = f.Element(sky + "flightNumber")!.Value,
                Departure = f.Element(sky + "departure")!.Value,
                Arrival = f.Element(sky + "arrival")!.Value,
                Price = decimal.Parse(f.Element(sky + "price")?.Value ?? "0"),
                Currency = f.Element(sky + "currency")!.Value,
                Duration = f.Element(sky + "duration")!.Value,
                DepartureTime = DateTime.Parse(f.Element(sky + "departureTime")?.Value ?? DateTime.MinValue.ToString()),
                ArrivalTime = DateTime.Parse(f.Element(sky + "arrivalTime")?.Value ?? DateTime.MinValue.ToString()),
                ProviderName = ProviderName
            });

        flights = flights.Where(x => x.DepartureTime.Date == request.DepartureDate.Date && x.Departure == request.Origin &&x.Arrival==request.Destination).ToList();

        foreach (var flight in flights)
        {
            if (cancellationToken.IsCancellationRequested) yield break;
            yield return flight;
        }
    }
}