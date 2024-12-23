using FlightSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.Specifications;
public class FlightSearchSpecification : ISpecification<Flight>
{
    private readonly string _origin;
    private readonly string _destination;
    private readonly DateTime _departureDate;
    private readonly int _passengerCount;

    public FlightSearchSpecification(
        string origin,
        string destination,
        DateTime departureDate,
        int passengerCount)
    {
        _origin = origin;
        _destination = destination;
        _departureDate = departureDate;
        _passengerCount = passengerCount;
    }

    public bool IsSatisfiedBy(Flight flight)
    {
        return flight.DepartureAirport.Code == _origin &&
               flight.ArrivalAirport.Code == _destination &&
               flight.Schedule.DepartureTime.Date == _departureDate.Date;
    }

    public Expression<Func<Flight, bool>> ToExpression()
    {
        return flight =>
            flight.DepartureAirport.Code == _origin &&
            flight.ArrivalAirport.Code == _destination &&
            flight.Schedule.DepartureTime.Date == _departureDate.Date;
    }
}