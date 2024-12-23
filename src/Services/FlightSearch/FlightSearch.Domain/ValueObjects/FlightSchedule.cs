using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record FlightSchedule
{
    public DateTime DepartureTime { get; }
    public DateTime ArrivalTime { get; }

    private FlightSchedule(DateTime departureTime, DateTime arrivalTime)
    {
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
    }

    public static FlightSchedule Create(DateTime departureTime, DateTime arrivalTime)
    {
        if (arrivalTime <= departureTime)
            throw new DomainException("Arrival time must be after departure time");

        return new FlightSchedule(departureTime, arrivalTime);
    }
}