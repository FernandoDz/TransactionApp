namespace TransactionUI.Models
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
    public class PaymentInfoResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<PaymentInfo> Response { get; set; }
    }
}
