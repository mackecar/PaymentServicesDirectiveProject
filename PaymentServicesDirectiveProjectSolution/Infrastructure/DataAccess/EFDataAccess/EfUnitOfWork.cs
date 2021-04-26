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

        private readonly PSDDbContext _context;
        private IDbContextTransaction _dbContextTransaction;

        public EfUnitOfWork()
        {
            _context = new PSDDbContext();
            UserRepository = new EFUserRepository(_context);
            TransactionRepository = new EFTransactionRepository(_context);
        }

        public async Task BeginTransactionAsync()
        {
            _dbContextTransaction = await _context.Database.BeginTransactionAsync();
        }

        public Task CommitTransactionAsync()
        {
            return _dbContextTransaction.CommitAsync();
        }

        public Task RollbackTransactionAsync()
        {
            return _dbContextTransaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TransactionRepository?.Dispose();
                    _context.Dispose();
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
