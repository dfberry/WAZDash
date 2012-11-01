


namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;

    public class ExceptionReponse<T> where T : Request
    {
        /// <summary>
        /// Exception raised in the request
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Request that raised the exception
        /// </summary>
        public T Request { get; set; }
    }
}
