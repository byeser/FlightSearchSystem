using Booking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.DTOs;
public record BookingResult(Guid BookingId, BookingStatus Status);