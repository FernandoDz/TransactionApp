using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.WebAPI.Models
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
   
}
