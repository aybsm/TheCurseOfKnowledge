using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Models;

namespace TheCurseOfKnowledge.Core.Interfaces.ExternalServices
{
    public interface IApiService
    {
        AuthenticationHeaderValue Authentication { get; set; }
        Task<ResultModel<TResult>> GetAsync<TResult>(string url);
        Task<ResultModel<TResult>> GetAsync<TResult>(string url, CancellationToken token);
        Task<ResultModel<TResult>> PostAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload);
        Task<ResultModel<TResult>> PostAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token);
        Task<ResultModel<string>> PostWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload);
        Task<ResultModel<string>> PostWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token);
        Task<ResultModel<string>> PostWithoutConvertAsync(string url, Dictionary<string, string> headers, HttpContent content, CancellationToken token);
        Task<ResultModel<TResult>> PutAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload);
        Task<ResultModel<TResult>> PutAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token);
        Task<ResultModel<string>> PutWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload);
        Task<ResultModel<string>> PutWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token);
    }
}
