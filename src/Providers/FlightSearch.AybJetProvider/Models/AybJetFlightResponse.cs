namespace FlightSearch.AybJetProvider.Models;
public class AybJetFlightResponse
{
    public required string FlightNumber { get; init; }
    public required string Departure { get; init; }
    public required string Arrival { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required string Duration { get; init; }
    public required DateTime DepartureTime { get; init; }
    public required DateTime ArrivalTime { get; init; }
}