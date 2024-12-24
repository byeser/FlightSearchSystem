using Booking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.DTOs;
public record BookingResult
{
    public Guid BookingId { get; }
    public BookingStatus Status { get; }

    public BookingResult(Guid bookingId, BookingStatus status)
    {
        BookingId = bookingId;
        Status = status;
    }
}