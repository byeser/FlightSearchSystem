using Booking.Domain.Repositories;
using Booking.Domain.ValueObjects;
using FlightSearch.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Repositories;
public class BookingRepository : IBookingRepository
{
    private static readonly List<Domain.Entities.Booking> _bookings = new();
    private readonly ILogger<BookingRepository> _logger;

    public BookingRepository(ILogger<BookingRepository> logger)
    {
        _logger = logger;
    }

    public async Task<Domain.Entities.Booking> GetByIdAsync(dynamic id, CancellationToken cancellationToken = default)
    {
        if (id is BookingId bookingId)
            return _bookings.FirstOrDefault(b => b.Id == bookingId);

        if (id is Guid guidId)
            return _bookings.FirstOrDefault(b => b.Id.Value == guidId);

        throw new ArgumentException("Invalid id type", nameof(id));
    }

    public async Task<Domain.Entities.Booking> GetByBookingIdAsync(BookingId id, CancellationToken cancellationToken = default)
    {
        return _bookings.FirstOrDefault(b => b.Id == id);
    }

    public async Task AddAsync(Domain.Entities.Booking booking, CancellationToken cancellationToken = default)
    {
        if (_bookings.Any(b => b.Id == booking.Id))
            throw new DomainException($"Booking {booking.Id} already exists");

        _bookings.Add(booking);
        _logger.LogInformation("Booking {BookingId} created successfully", booking.Id.Value);
    }

    public async Task UpdateAsync(Domain.Entities.Booking booking, CancellationToken cancellationToken = default)
    {
        var index = _bookings.FindIndex(b => b.Id == booking.Id);
        if (index == -1)
            throw new NotFoundException($"Booking {booking.Id} not found");

        _bookings[index] = booking;
        _logger.LogInformation("Booking {BookingId} updated successfully", booking.Id.Value);
    }

    public async Task<Booking.Domain.Entities.Booking> GetByIdAsync(Booking.Domain.ValueObjects.BookingId id, CancellationToken cancellationToken)
    {
        return _bookings.FirstOrDefault(b => b.Id == id);
    }
}