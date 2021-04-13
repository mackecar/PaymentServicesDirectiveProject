using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DataAccess.EFDataAccess;
using Microsoft.EntityFrameworkCore.Design;

namespace Applications.TestAndUtility.EFSQLRunner
{
    public class ContextFactory : IDesignTimeDbContextFactory<PSDDbContext>
    {
        public PSDDbContext CreateDbContext(string[] args)
        {
            EfConnectionSettings.ConfigureSqlServerConnection(@"Data Source=localhost;Initial Catalog=PSD;user id=sa;password=Beograd011!");
            return new PSDDbContext();
        }
    }
}
