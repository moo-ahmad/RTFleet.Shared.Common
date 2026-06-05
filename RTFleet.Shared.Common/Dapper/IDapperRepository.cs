using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTFleet.Shared.Common.Dapper
{
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<IEnumerable<T>> QueryProcedureAsync<T>(string sql, object parameters = null);
        Task<bool> ExecuteAsync(string sql, object parameters = null);
    }
}
