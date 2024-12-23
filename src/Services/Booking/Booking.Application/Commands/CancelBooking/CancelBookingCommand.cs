using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.CancelBooking;
public record CancelBookingCommand : IRequest
{
    public Guid Id { get; init; }
    public CancelBookingCommand(Guid id) => Id = id;
}