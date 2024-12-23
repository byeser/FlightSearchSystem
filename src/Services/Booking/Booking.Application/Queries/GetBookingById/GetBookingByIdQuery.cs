using Booking.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries.GetBookingById;
public record GetBookingByIdQuery : IRequest<BookingResponse>
{
    public Guid Id { get; init; }
    public GetBookingByIdQuery(Guid id) => Id = id;
}