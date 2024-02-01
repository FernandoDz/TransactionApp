using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.WebAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public Payment()
        {
            PaymentDate = DateTime.Now;
        }
    }

    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(payment => payment.ClientId).NotEmpty();
            RuleFor(payment => payment.CardNumber).NotEmpty().CreditCard();
            RuleFor(payment => payment.PaymentDate).NotEmpty();
            RuleFor(payment => payment.Amount).GreaterThan(0);
        }
    }

}
