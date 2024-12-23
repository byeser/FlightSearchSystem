using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities;
public class BookingItem
{
    public Guid Id { get; private set; }
    public string PassengerName { get; private set; }
    public string PassengerSurname { get; private set; }
    public string? PassportNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }

    public static BookingItem Create(
        string passengerName,
        string passengerSurname,
        DateTime dateOfBirth,
        decimal price,
        string currency,
        string? passportNumber = null)
    {
        if (string.IsNullOrEmpty(passengerName))
            throw new DomainException("Passenger name is required");

        if (string.IsNullOrEmpty(passengerSurname))
            throw new DomainException("Passenger surname is required");

        return new BookingItem
        {
            Id = Guid.NewGuid(),
            PassengerName = passengerName,
            PassengerSurname = passengerSurname,
            DateOfBirth = dateOfBirth,
            Price = price,
            Currency = currency,
            PassportNumber = passportNumber
        };
    }
}