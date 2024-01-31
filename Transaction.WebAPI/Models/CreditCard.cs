using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.WebAPI.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public string Number { get; set; }
        public string HolderFirstName { get; set; }
        public string HolderLastName { get; set; }
        public int Top_Aux { get; set; }
    }

    public class CreditCardResponse
    {
        public string Mensaje { get; set; }
        public IEnumerable<CreditCard> Response { get; set; }
    }
}
