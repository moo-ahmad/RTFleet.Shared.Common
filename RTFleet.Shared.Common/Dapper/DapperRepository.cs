using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTFleet.Shared.Common.Dapper
{
    public class DapperRepository : IDapperRepository
    {
        private readonly string _conncetionStrings;
        public DapperRepository(string conncetionStrings)
        {
            _conncetionStrings = conncetionStrings;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var QueryResponse = await connection.QueryAsync<T>(sql: sql, param: parameters, commandType: CommandType.Text, commandTimeout: 180);
                    return QueryResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var QueryResponse = await connection.QueryAsync<T>(sql: sql, param: parameters, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    return QueryResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var QueryResponse = await connection.ExecuteAsync(sql: sql, param: parameters, commandType: CommandType.Text, commandTimeout: 180);
                    return QueryResponse > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_conncetionStrings);
        }

    }
}
