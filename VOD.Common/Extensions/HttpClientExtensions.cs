using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Exceptions;

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
        private static async Task<TResponse> DeserializeResponse<TResponse>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            return responseStream.ReadAndDeserializeFromJson<TResponse>();
        }
        private static async Task CheckStatusCodes(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                object validationErrors = null;
                var message = string.Empty;
                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var errorStream = await response.Content.ReadAsStreamAsync();
                    validationErrors = errorStream.ReadAndDeserializeFromJson();
                    message = "Could not process the entity.";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    message = "Bad request.";
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                    message = "Access denied.";
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    message = "Could not find the entity.";
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    message = "Not Logged In.";

                throw new HttpResponseException(response.StatusCode, message,
                    validationErrors);
            }
            else response.EnsureSuccessStatusCode();
        }

    }
}
