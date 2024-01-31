namespace TransactionUI.Models
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

    public class PurchaseResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<Purchase> Response { get; set; }
    }
}
