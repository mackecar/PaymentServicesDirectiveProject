using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.EFDataAccess.Repositories
{
    public class EFTransactionRepository : EfRepository<Transaction>, ITransactionRepository
    {
        public EFTransactionRepository(PSDDbContext context) : base(context)
        {
        }


        public async Task<List<Transaction>> GetUsersTransactions(int userId)
        {
            return await DbSet.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Transaction> GetTransaction(int transactionId)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task Insert(Transaction transaction)
        {
            await DbSet.AddAsync(transaction);
        }

        public async Task<bool> Delete(Transaction transaction)
        {
            DbSet.Remove(transaction);
            return await Task.FromResult(true);
        }
    }
}
