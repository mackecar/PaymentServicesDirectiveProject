using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Repositories;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable  
    {
        IUserRepository UserRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
