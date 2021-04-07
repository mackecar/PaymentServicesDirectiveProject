using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Infrastructure.DataAccess.EFDataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.EFDataAccess
{
    public class PSDDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public PSDDbContext() : base(EfConnectionSettings.DbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
