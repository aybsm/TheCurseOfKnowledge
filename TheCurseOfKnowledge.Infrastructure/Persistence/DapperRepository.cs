using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Interfaces.Persistence;

namespace TheCurseOfKnowledge.Infrastructure.Persistence
{
    public class DapperRepository : IDbRepository
    {
        readonly IDbConnection __cConnection;
        public DapperRepository(IDbConnection connection)
            => __cConnection = connection;
        public async Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.QueryAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        public async Task<TModel> QueryFirstAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.QueryFirstAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        public async Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.QueryFirstOrDefaultAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        public async Task<TModel> QuerySingleAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.QuerySingleAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        public async Task<TModel> QuerySingleOrDefaultAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.QuerySingleOrDefaultAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.ExecuteAsync(sql, param, transaction, commandtimeout, commandtype);
        public async Task<TModel> ExecuteScalarAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
            => await __cConnection.ExecuteScalarAsync<TModel>(sql, param, transaction, commandtimeout, commandtype);
        //public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null)
        //    => await __cConnection.QueryMultipleAsync(sql, param, transaction, commandtimeout, commandtype);
        public IDbTransaction BeginTransaction()
        {
            if (__cConnection.State == ConnectionState.Closed)
                __cConnection.Open();
            return __cConnection.BeginTransaction();
        }
        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
            if (__cConnection.State == ConnectionState.Open)
                __cConnection.Close();
        }
        public void RollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
            if (__cConnection.State == ConnectionState.Open)
                __cConnection.Close();
        }
    }
}
