using Booking.Domain.Repositories;
using Booking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Repositories;
public class BookingRepository : IBookingRepository
{
    private static readonly List<Booking.Domain.Entities.Booking> _bookings = new();

    public async Task<Booking.Domain.Entities.Booking> GetByIdAsync(dynamic id, CancellationToken cancellationToken = default)
    {
        if (id is BookingId bookingId)
            return await GetByIdAsync(bookingId, cancellationToken);

        throw new ArgumentException("Invalid id type", nameof(id));
    }
    public async Task<Booking.Domain.Entities.Booking> GetByIdAsync(Booking.Domain.ValueObjects.BookingId id, CancellationToken cancellationToken)
    {
        return _bookings.FirstOrDefault(b => b.Id == id);
    }

    public async Task AddAsync(Booking.Domain.Entities.Booking booking, CancellationToken cancellationToken)
    {
        _bookings.Add(booking);
    }

    public async Task UpdateAsync(Booking.Domain.Entities.Booking booking, CancellationToken cancellationToken)
    {
        var index = _bookings.FindIndex(b => b.Id == booking.Id);
        if (index != -1)
            _bookings[index] = booking;
    }
}