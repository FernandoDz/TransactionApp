namespace TransactionUI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; 
        public decimal Amount { get; set; }
    }
    public class PaymentResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<Payment> Response { get; set; }
    }
}
