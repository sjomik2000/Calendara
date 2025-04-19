using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Database
{
    public class DbInitializer
    {
        private readonly IDatabaseConnection _databaseConnection;

        public DbInitializer(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _databaseConnection.CreateConnectionAsync(); 
            
        }
    }
}
