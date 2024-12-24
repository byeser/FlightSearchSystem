using Booking.Application.DTOs;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.Events;
using Booking.Domain.Repositories;
using Booking.Domain.ValueObjects;
using FlightSearch.MessageBus;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Booking.Application.Commands.CreateBooking;
public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResult>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightService _flightService;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IFlightService flightService,
        IMessageBus messageBus,
        ILogger<CreateBookingCommandHandler> logger)
    {
        _bookingRepository = bookingRepository;
        _flightService = flightService;
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task<BookingResult> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var flightInfo = await _flightService.GetFlightByNumberAsync(
                request.FlightNumber,
                cancellationToken);

            // Passengers create metodu ile oluşturuluyor
            var passengers = request.Passengers.Select(p =>
                Passenger.Create(
                    p.FirstName,
                    p.LastName,
                    p.DateOfBirth,
                    p.PassportNumber
                )).ToList();

            var totalPrice = Money.Create(
                flightInfo.Price * passengers.Count,
                Currency.FromCode(flightInfo.Currency));

            var flightReference = FlightReference.Create(
                flightInfo.FlightNumber,
                flightInfo.Provider);

            var booking =Domain.Entities.Booking.Create(
                flightReference,
                passengers,
                totalPrice);

            await _bookingRepository.AddAsync(booking, cancellationToken);
            await _messageBus.PublishAsync(new BookingCreatedDomainEvent(booking), cancellationToken);

            return new BookingResult(booking.Id.Value, BookingStatus.Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for flight {FlightNumber}", request.FlightNumber);
            throw;
        }
    }
}