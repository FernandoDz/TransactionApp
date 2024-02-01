namespace Transaction.WebAPI.Models
{
    public class PaymentInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Payment { get; set; }
    }
   
}
