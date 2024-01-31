using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface IPurchaseInfoService
    {
        Task<IEnumerable<PurchaseInfo>> GetById(int id);
    }
}
