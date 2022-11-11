using FluentValidation;
using Microsoft.EntityFrameworkCore;
using REDChallenge.Application.Models;
using REDChallenge.Persistance;

namespace REDChallenge.Application.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderModel>
    {
        private readonly REDChallengeContext _context;
        public CreateOrderValidator(REDChallengeContext context)
        {
            _context = context;

            RuleFor(x => x.OrderTypeId)
                .NotNull()
                .WithMessage("Order type is a necessary field")
                .MustAsync(async (orderTypeId, cancellation) =>
                {
                    return await _context.OrderType.AnyAsync(ot => ot.Id == orderTypeId, cancellation);
                })
                .WithMessage("Invalid provided order type id");

            RuleFor(x => x.CustomerId)
                .NotNull()
                .WithMessage("Customer is a necessary field")
                .MustAsync(async (customerId, cancellation) =>
                {
                    return await _context.Customer.AnyAsync(ot => ot.Id == customerId, cancellation);
                })
                .WithMessage("Invalid provided customer id");
        }
    }
}
