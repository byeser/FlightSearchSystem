using FlightSearch.Domain.Events;
using FlightSearch.Domain.ValueObjects;
using FlightSearch.Shared.Domain;
using FlightSearch.Shared.Exceptions;

namespace FlightSearch.Domain.Entities;
public class Flight : IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public string FlightNumber { get; private set; }
    public Airport DepartureAirport { get; private set; }
    public Airport ArrivalAirport { get; private set; }
    public Money Price { get; private set; }
    public Duration Duration { get; private set; }
    public FlightSchedule Schedule { get; private set; }
    public Provider Provider { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Flight() { }

    public static Flight Create(
        string flightNumber,
        Airport departure,
        Airport arrival,
        Money price,
        Duration duration,
        FlightSchedule schedule,
        Provider provider)
    {
        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = flightNumber,
            DepartureAirport = departure,
            ArrivalAirport = arrival,
            Price = price,
            Duration = duration,
            Schedule = schedule,
            Provider = provider
        };

        flight.AddDomainEvent(new FlightCreatedDomainEvent(flight));

        return flight;
    }

    private void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount < 0)
            throw new DomainException("Price cannot be negative");

        Price = newPrice;
        AddDomainEvent(new FlightPriceUpdatedDomainEvent(this, newPrice));
    }

    public void UpdateSchedule(FlightSchedule newSchedule)
    {
        Schedule = newSchedule;
        AddDomainEvent(new FlightScheduleUpdatedDomainEvent(this, newSchedule));
    }

    public bool IsAvailable(DateTime date)
    {
        return Schedule.DepartureTime.Date == date.Date;
    }

    public bool HasCapacity(int requiredSeats)
    { 
        return true;
    }
}