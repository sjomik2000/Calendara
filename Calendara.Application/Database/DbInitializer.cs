using Calendara.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Database
{
    public class DbInitializer
    {
        private readonly DatabaseConnection _dbContext;

        public DbInitializer(DatabaseConnection dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _dbContext.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception occurred: {ex}");
            }

        }
    }
}
