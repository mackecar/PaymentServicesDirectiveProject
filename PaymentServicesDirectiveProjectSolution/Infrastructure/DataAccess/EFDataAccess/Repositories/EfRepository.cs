using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.EFDataAccess.Repositories
{
    public abstract class EfRepository<TEntity> where TEntity : class
    {
        protected PSDDbContext DbContext;
        protected DbSet<TEntity> DbSet;

        protected EfRepository(PSDDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            try
            {
                DbSet.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
