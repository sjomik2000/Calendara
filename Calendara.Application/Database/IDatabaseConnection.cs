using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendara.Application.Database
{
    public interface IDatabaseConnection
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}
