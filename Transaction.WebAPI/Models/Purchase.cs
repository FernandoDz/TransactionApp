using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.WebAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public string PurchaseDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string AuthorizationNumber { get; set; }
        public string Month { get; set; }

    }
    public class PurchaseValidator : AbstractValidator<Purchase>
    {
        public PurchaseValidator()
        {
            RuleFor(purchase => purchase.ClientId).NotEmpty().WithMessage("Client Id is required.");
            RuleFor(purchase => purchase.CardNumber).NotEmpty().WithMessage("Card Number is required.")
                                                      .CreditCard().WithMessage("'Card Number' is not a valid credit card number.");
            RuleFor(purchase => purchase.PurchaseDate).NotEmpty().WithMessage("Purchase Date is required")
                                                      .Must(BeAValidDate).WithMessage("'Purchase Date' must be a valid date.");
            RuleFor(purchase => purchase.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(purchase => purchase.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
