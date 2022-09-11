using FluentValidation;

namespace Booking.Cancun.Domain.Entities.Contracts;

internal class BookingOrderDomainCreateContract : AbstractValidator<BookingOrderDomain>
{
    public BookingOrderDomainCreateContract()
    {
        RuleFor(c => c.StartDate)
            .NotNull().WithMessage("Is Required")
            .NotEmpty().WithMessage("Is Required")
            .GreaterThan(DateTime.Today).WithMessage("Should be in future")
            .LessThan(DateTime.Today.AddDays(30))
                .WithMessage("Should be in the next 30 days");

        RuleFor(c => c.EndDate)
            .NotNull().WithMessage("Is Required")
            .NotEmpty().WithMessage("Is Required")
            .GreaterThan(DateTime.Today).WithMessage("Should be in future")
            .GreaterThan(p => p.StartDate).WithMessage("Should be greater than StartDate")
            .LessThanOrEqualTo(p => p.StartDate.AddDays(3))
                .WithMessage("Should not stay more than 3 days");

        RuleFor(c => c.Email)
            .NotNull().WithMessage("Is Required")
            .NotEmpty().WithMessage("Is Required")
            .MaximumLength(100).WithMessage("Size should be a less than 100 carach characters")
            .EmailAddress().WithMessage("Should be a valid Email");
    }
}
