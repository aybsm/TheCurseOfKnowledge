using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Core.Interfaces.Persistence
{
    public interface IDbRepository
    {
        Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<TModel> QueryFirstAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<TModel> QuerySingleAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<TModel> QuerySingleOrDefaultAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        Task<TModel> ExecuteScalarAsync<TModel>(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        //Task<GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandtimeout = null, CommandType? commandtype = null);
        IDbTransaction BeginTransaction();
        void CommitTransaction(IDbTransaction transaction);
        void RollbackTransaction(IDbTransaction transaction);
    }
}
