using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task InsertAsync(Transaction transaction);
        Task<List<Transaction>> GetUsersTransactions(int userId);
    }
}
