using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace VOD.Common.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        #region Properties
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        #endregion

        #region Constructor
        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _cancellationToken = _cancellationTokenSource.Token;
        }
        #endregion

        #region Methods

        #endregion
    }
}
