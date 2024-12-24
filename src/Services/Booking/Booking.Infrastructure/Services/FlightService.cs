using Booking.Application.DTOs;
using Booking.Application.Exceptions;
using Booking.Application.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Booking.Infrastructure.Services;
public class FlightService : IFlightService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FlightService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public FlightService(HttpClient httpClient, ILogger<FlightService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<FlightDto> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/flight/{flightNumber}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var flight = JsonSerializer.Deserialize<FlightDto>(content, _jsonOptions);

            if (flight == null)
                throw new NotFoundException($"Flight {flightNumber} not found");

            return flight;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error getting flight {FlightNumber}", flightNumber);
            throw new ServiceException("Flight service is unavailable", ex);
        }
    }

    public async Task<bool> CheckAvailabilityAsync(string flightNumber, int passengerCount, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/api/flight/{flightNumber}/availability?passengerCount={passengerCount}",
                cancellationToken);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error checking availability for flight {FlightNumber}", flightNumber);
            return false;
        }
    }
}