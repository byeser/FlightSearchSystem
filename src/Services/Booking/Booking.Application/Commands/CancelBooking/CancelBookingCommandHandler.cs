using Booking.Domain.Repositories;
using FlightSearch.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.CancelBooking;
public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
{
    private readonly IBookingRepository _repository;

    public CancelBookingCommandHandler(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (booking == null)
            throw new NotFoundException($"Booking {request.Id} not found");

        booking.Cancel();
        await _repository.UpdateAsync(booking, cancellationToken);
    }
}
