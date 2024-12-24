using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities;
public class Passenger
{
    private Passenger() { }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? PassportNumber { get; private set; }

    public static Passenger Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string? passportNumber = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");

        return new Passenger
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            PassportNumber = passportNumber
        };
    }
}