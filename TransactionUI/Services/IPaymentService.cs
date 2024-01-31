using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetById(int id);
        Task<bool> CreatePayment(Payment payment);
    }
}
