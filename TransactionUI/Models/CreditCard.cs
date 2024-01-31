namespace TransactionUI.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public string Number { get; set; }
        public string HolderFirstName { get; set; }
        public string HolderLastName { get; set; }
    }
    public class CreditCardResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<CreditCard> Response { get; set; }
    }
}
