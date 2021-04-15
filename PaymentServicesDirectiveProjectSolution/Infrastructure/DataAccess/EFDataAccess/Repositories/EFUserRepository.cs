using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.DataAccess.EFDataAccess.Repositories
{
    public class EFUserRepository : EfRepository<User>, IUserRepository
    {
        public EFUserRepository(PSDDbContext context) : base(context)
        {
        }

        public async Task InsertAsync(User user)
        {
            await DbSet.AddAsync(user);
        }

        public async Task<User> GetUserByPersonalNumberAsync(string personalNumber)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.PersonalNumber == personalNumber);
        }

        public async Task Insert(User user)
        {
            await DbSet.AddAsync(user);
        }

        public async Task<bool> Delete(User user)
        {
            DbSet.Remove(user);
            return await Task.FromResult(true);
        }
    }
}
