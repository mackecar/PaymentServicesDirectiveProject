using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
