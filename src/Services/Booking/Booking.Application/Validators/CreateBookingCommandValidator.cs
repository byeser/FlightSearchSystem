using Booking.Application.Commands.CreateBooking;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Validators;
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.FlightNumber).NotEmpty();
        RuleFor(x => x.Passengers).NotEmpty();
        RuleForEach(x => x.Passengers).ChildRules(passenger => {
            passenger.RuleFor(p => p.FirstName).NotEmpty();
            passenger.RuleFor(p => p.LastName).NotEmpty();
            passenger.RuleFor(p => p.DateOfBirth).NotEmpty();
        });
    }
}