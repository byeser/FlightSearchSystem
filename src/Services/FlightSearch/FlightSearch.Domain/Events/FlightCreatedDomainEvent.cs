using FlightSearch.Domain.Entities;
using FlightSearch.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.Events;
public record FlightCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Flight Flight { get; }

    public FlightCreatedDomainEvent(Flight flight)
    {
        Flight = flight;
    }
}