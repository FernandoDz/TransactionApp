namespace TransactionUI.Models
{
    public class PurchaseInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CardNumber { get; set; }
        public string AuthorizationNumber { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Charge { get; set; }
    }
    public class PurchaseInfoResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<PurchaseInfo> Response { get; set; }
    }
}
