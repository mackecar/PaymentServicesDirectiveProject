using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable  
    {
        IUserRepository UserRepository { get; }
        Task SaveChangesAsync();
    }
}
