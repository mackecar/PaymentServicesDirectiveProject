using System.Threading.Tasks;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess.Repositories;

namespace Infrastructure.DataAccess.EFDataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        private PSDDbContext Context;

        public EfUnitOfWork()
        {
            Context = new PSDDbContext();
            UserRepository = new EFUserRepository(Context);
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
                    Context.Dispose();
                    Context = null;
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
