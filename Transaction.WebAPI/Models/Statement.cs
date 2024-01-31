using System.ComponentModel.DataAnnotations.Schema;

namespace Transaction.WebAPI.Models
{
    public class Statement
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal BonifiableInterest { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
