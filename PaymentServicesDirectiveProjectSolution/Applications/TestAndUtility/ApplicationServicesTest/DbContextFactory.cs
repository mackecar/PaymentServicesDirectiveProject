using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DataAccess.EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Applications.TestAndUtility.ApplicationServicesTest
{
    public class DbContextFactory : IDesignTimeDbContextFactory<PSDDbContext>
    {
        public PSDDbContext CreateDbContext(string[] args)
        {
            EfConnectionSettings.ConfigureSqlServerConnection(@"Data Source=localhost;Initial Catalog=PSD;user id=sa;password=Beograd011!");

            return new PSDDbContext();
        }
    }
}
