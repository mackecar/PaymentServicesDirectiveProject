using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.EFDataAccess
{
    public static class EfConnectionSettings
    {
        internal static DbContextOptions<PSDDbContext> DbContextOptions;

        /// <exception cref="InvalidOperationException">Connection already configured.</exception>
        public static void ConfigureSqlServerConnection(string connectionString)
        {
            if (DbContextOptions != null)
            {
                throw new InvalidOperationException("Connection already configured.");
            }
            var optionsBuilder = new DbContextOptionsBuilder<PSDDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            DbContextOptions = optionsBuilder.Options;
        }
    }
}
