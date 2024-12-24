using Booking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services;
public interface IFlightService
{
    Task<FlightDto> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default);
    Task<bool> CheckAvailabilityAsync(string flightNumber, int passengerCount, CancellationToken cancellationToken = default);
}