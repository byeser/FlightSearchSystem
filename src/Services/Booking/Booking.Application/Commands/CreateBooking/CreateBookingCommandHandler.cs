using Booking.Application.DTOs;
using Booking.Domain.Enums;
using Booking.Domain.Events;
using Booking.Domain.Repositories;
using Booking.Domain.ValueObjects;
using FlightSearch.Domain.Repositories;
using FlightSearch.Domain.ValueObjects;
using FlightSearch.MessageBus;
using FlightSearch.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Booking.Application.Commands.CreateBooking;
public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResult>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    

    public async Task<BookingResult> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var flight = await _flightRepository.GetByFlightNumberAsync(
                request.FlightNumber,
                cancellationToken);

            if (flight == null)
                throw new NotFoundException($"Flight {request.FlightNumber} not found");

            var passengers = request.Passengers
                .Select(p => Passenger.Create(
                    p.FirstName,
                    p.LastName,
                    p.DateOfBirth,
                    p.PassportNumber))
                .ToList();

            var totalPrice = Money.Create(
                flight.Price.Amount * passengers.Count,
                flight.Price.Currency);

            var booking = Booking.Domain.Entities.Booking.Create(FlightId.FromGuid(flight.Id), passengers, totalPrice);

            await _bookingRepository.AddAsync(booking, cancellationToken);
            await _messageBus.PublishAsync(new BookingCreatedDomainEvent(booking), cancellationToken);

            return new BookingResult(booking.Id.Value, BookingStatus.Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            throw;
        }
    }
}