using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface ICreditCardService
    {
        Task<IEnumerable<CreditCard>> GetAllCreditCards();
    }
}
