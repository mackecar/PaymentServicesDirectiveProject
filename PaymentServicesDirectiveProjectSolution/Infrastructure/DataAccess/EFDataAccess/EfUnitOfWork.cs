using System.Threading.Tasks;
using Core.Domain.Repositories;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataAccess.EFDataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        private PSDDbContext Context;
        private IDbContextTransaction Transaction;

        public EfUnitOfWork()
        {
            Context = new PSDDbContext();
            UserRepository = new EFUserRepository(Context);
            TransactionRepository = new EFTransactionRepository(Context);
        }

        public async Task BeginTransactionAsync()
        {
            Transaction = await Context.Database.BeginTransactionAsync();
        }

        public Task CommitTransactionAsync()
        {
            return Transaction.CommitAsync();
        }

        public Task RollbackTransactionAsync()
        {
            return Transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Transaction?.Dispose();
                    Context.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}
