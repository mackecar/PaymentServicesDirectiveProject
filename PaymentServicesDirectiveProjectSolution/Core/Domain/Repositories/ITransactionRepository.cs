using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetUsersTransactions(int userId);
        Task<Transaction> GetTransaction(int transactionId);
    }
}
