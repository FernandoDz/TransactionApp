using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface IPaymentInfoService
    {
        Task<IEnumerable<PaymentInfo>> GetById(int id);
    }
}
