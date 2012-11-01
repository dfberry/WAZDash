using System;
using System.Net;

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    public abstract class Request
    {

        /// <summary>
        /// Called Back On Successful Request
        /// </summary>
        public AsyncCallback Callback { get; set; }

        /// <summary>
        /// Callback for exceptions
        /// </summary>
        public AsyncCallback ExceptionCallback { get; set; }
    }
}
