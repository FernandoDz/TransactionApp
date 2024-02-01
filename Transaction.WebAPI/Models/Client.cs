using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transaction.WebAPI.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<CreditCard> CreditCard { get; set; }
        public virtual List<Statement> Statement { get; set; }
        public virtual List<Purchase> Purchase { get; set; }
        public virtual List<Payment> Payment { get; set; }
        public virtual List<PurchaseInfo> Transaction { get; set; }
    }
}
