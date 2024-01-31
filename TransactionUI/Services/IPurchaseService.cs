using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetById(int id);
        Task<bool> CreatePurchase(Purchase purchase);
    }
}
