using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record Provider
{
    public string Name { get; }

    private Provider(string name) => Name = name;

    public static Provider Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Provider name cannot be empty");

        return new Provider(name);
    }
}