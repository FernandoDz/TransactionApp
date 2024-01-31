using TransactionUI.Models;

namespace TransactionUI.Services
{
    public interface IStatementService
    {
        Task<IEnumerable<Statement>> GetById(int id);
    }
}
