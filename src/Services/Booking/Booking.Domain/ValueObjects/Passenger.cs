using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;
public record Passenger
{
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
    public string? PassportNumber { get; }

    private Passenger(string firstName, string lastName, DateTime dateOfBirth, string? passportNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PassportNumber = passportNumber;
    }

    public static Passenger Create(string firstName, string lastName, DateTime dateOfBirth, string? passportNumber = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");

        return new Passenger(firstName, lastName, dateOfBirth, passportNumber);
    }
}