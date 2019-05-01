using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Extensions
{
    public static class HttpClientExtensions
    {
        private static HttpRequestMessage CreateRequestHeaders(this string uri, HttpMethod httpMethod, string token = "")
        {
            var requestHeader = new HttpRequestMessage(httpMethod, uri);
            requestHeader.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!token.IsNullOrEmptyOrWhiteSpace()) requestHeader.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            if (httpMethod.Equals(HttpMethod.Get)) requestHeader.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            return requestHeader;
        }
        private static async Task<StreamContent> SerializeRequestContentAsync<TRequest>(this TRequest content)
        {
            var stream = new MemoryStream();
            await stream.SerializeToJsonAndWriteAsync(content, new UTF8Encoding(), 1024, true);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamContent(stream);
        }
        private static async Task<HttpRequestMessage> CreateRequestContent<TRequest>(this HttpRequestMessage requestMessage, TRequest content)
        {
            requestMessage.Content = await content.SerializeRequestContentAsync();
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestMessage;
        }
    }
}
