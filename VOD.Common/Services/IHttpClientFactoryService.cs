using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public interface IHttpClientFactoryService
    {
        Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class;
        Task<TResponse> GetAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class;
        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest content, string uri, string serviceName, string token = "") where TResponse : class where TRequest : class;
    }
}
