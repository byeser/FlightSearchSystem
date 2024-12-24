using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.CreateBooking;
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required")
            .MaximumLength(10).WithMessage("Flight number cannot exceed 10 characters");

        RuleFor(x => x.Passengers)
            .NotEmpty().WithMessage("At least one passenger is required")
            .Must(p => p.Count <= 9).WithMessage("Maximum 9 passengers allowed per booking");

        RuleForEach(x => x.Passengers).ChildRules(passenger =>
        {
            passenger.RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            passenger.RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            passenger.RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .Must(dob => dob <= DateTime.Today)
                .WithMessage("Date of birth cannot be in the future");
        });
    }
}