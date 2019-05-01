using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

    }
}
