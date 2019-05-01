using System;
using System.Net;

namespace VOD.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        #region Properties
        public HttpStatusCode HttpStatusCode { get; }
        public object ValidationErrors { get; }
        #endregion

        #region Constructors
        public HttpResponseException(HttpStatusCode status, string message, object validationErrors) : base(message)
        {
            HttpStatusCode = status;
            ValidationErrors = validationErrors;
        }
        public HttpResponseException(HttpStatusCode status, string message) : this(status, message, null)
        {
            HttpStatusCode = status;
        }
        public HttpResponseException(HttpStatusCode status) : this(status, string.Empty, null)
        {
            HttpStatusCode = status;
        }
        #endregion
    }
}
