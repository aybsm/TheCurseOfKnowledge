using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Interfaces.Authentications;
using TheCurseOfKnowledge.Core.Interfaces.ExternalServices;
using TheCurseOfKnowledge.Core.Models;

namespace TheCurseOfKnowledge.Infrastructure.ExternalServices
{
    public class ApiService : IApiService
    {
        readonly HttpClient __client;
        IAuthenticationProvider __authentication;
        public ApiService(HttpClient client, IAuthenticationProvider authentication = default(IAuthenticationProvider))
        {
            __client = client;
            __authentication = authentication;
        }
        public async Task<ResultModel<TResult>> GetAsync<TResult>(string url)
            => await GetAsync<TResult>(url, default(CancellationToken));
        public async Task<ResultModel<TResult>> GetAsync<TResult>(string url, CancellationToken token)
        {
            var resultmodel = new ResultModel<TResult>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync());
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new WebException(message: result.ToString());

                        var responsemodel = JsonConvert.DeserializeObject<TResult>(result.ToString());
                        resultmodel.Model = responsemodel;
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
        public async Task<ResultModel<TResult>> PostAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload)
            => await PostAsync<TModel, TResult>(url, headers, payload, default(CancellationToken));
        public async Task<ResultModel<TResult>> PostAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token)
        {
            var resultmodel = new ResultModel<TResult>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(
                        content: JsonConvert.SerializeObject(payload),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    if (headers?.Count > 0)
                        foreach (var map in headers)
                            request.Headers.Add(map.Key, map.Value);
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync());
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                            throw new WebException(message: result.ToString());

                        //Console.WriteLine($"success: {result.ToString()}");

                        var responsemodel = JsonConvert.DeserializeObject<TResult>(result.ToString());
                        resultmodel.Model = responsemodel;
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
        public async Task<ResultModel<string>> PostWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload)
            => await PostWithoutConvertAsync<TModel>(url, headers, payload, default(CancellationToken));
        public async Task<ResultModel<string>> PostWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token)
        {
            var resultmodel = new ResultModel<string>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(
                        content: JsonConvert.SerializeObject(payload),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    if (headers?.Count > 0)
                        foreach (var map in headers)
                            request.Headers.Add(map.Key, map.Value);
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync());
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                            throw new WebException(message: result.ToString());

                        //Console.WriteLine($"success: {result.ToString()}");

                        resultmodel.Model = result.ToString();
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
        public async Task<ResultModel<string>> PostWithoutConvertAsync(string url, Dictionary<string, string> headers, HttpContent content, CancellationToken token)
        {
            var resultmodel = new ResultModel<string>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = content,
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    if (headers?.Count > 0)
                        foreach (var map in headers)
                            request.Headers.Add(map.Key, map.Value);
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync(token));
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                            throw new WebException(message: result.ToString());

                        resultmodel.Model = result.ToString();
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
        public async Task<ResultModel<TResult>> PutAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload)
            => await PutAsync<TModel, TResult>(url, headers, payload, default(CancellationToken));
        public async Task<ResultModel<TResult>> PutAsync<TModel, TResult>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token)
        {
            var resultmodel = new ResultModel<TResult>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = new StringContent(
                        content: JsonConvert.SerializeObject(payload),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    if (headers?.Count > 0)
                        foreach (var map in headers)
                            request.Headers.Add(map.Key, map.Value);
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync());
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new WebException(message: result.ToString());

                        //Console.WriteLine($"success: {result.ToString()}");

                        var responsemodel = JsonConvert.DeserializeObject<TResult>(result.ToString());
                        resultmodel.Model = responsemodel;
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
        public async Task<ResultModel<string>> PutWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload)
            => await PutWithoutConvertAsync<TModel>(url, headers, payload, default(CancellationToken));
        public async Task<ResultModel<string>> PutWithoutConvertAsync<TModel>(string url, Dictionary<string, string> headers, TModel payload, CancellationToken token)
        {
            var resultmodel = new ResultModel<string>();
            try
            {
                using (var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = new StringContent(
                        content: JsonConvert.SerializeObject(payload),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                })
                {
                    if (__authentication != default(IAuthenticationProvider))
                        request.Headers.Authorization = await __authentication.GetTokenAsync();
                    if (headers?.Count > 0)
                        foreach (var map in headers)
                            request.Headers.Add(map.Key, map.Value);
                    using (var response = await __client.SendAsync(request, token))
                    {
                        var result = new StringBuilder(await response.Content.ReadAsStringAsync());
                        resultmodel.IsSuccess = true;
                        resultmodel.StatusCode = response.StatusCode;
                        if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                            throw new WebException(message: result.ToString());

                        //Console.WriteLine($"success: {result.ToString()}");

                        resultmodel.Model = result.ToString();
                        return resultmodel;
                    }
                }
            }
            catch (WebException wexc)
            {
                resultmodel.Exception = wexc;
            }
            catch (Exception exc)
            {
                resultmodel.IsSuccess = false;
                resultmodel.Exception = exc;
            }
            return resultmodel;
        }
    }
}
