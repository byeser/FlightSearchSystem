using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.DTOs;
public record PassengerDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public string? PassportNumber { get; init; }
}
