using Booking.Domain.ValueObjects;
using FlightSearch.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories;
public interface IBookingRepository : IRepository<Entities.Booking>
{
    Task<Entities.Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Booking booking, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.Booking booking, CancellationToken cancellationToken = default);
}