///////////////////////////////////////////////////////////////////////////////
// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : InternalRequestState.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="InternalRequestState.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Net;

    /// <summary>
    /// Request State To Pass To Async Http Web Requests
    /// </summary>
    internal class InternalRequestState<T>
    {
        /// <summary>
        /// Gets or sets the request state to return
        /// </summary>
        public T ExternalRequestState { get; set; }

        /// <summary>
        /// Gets or sets the information about the httpwebrequest that was made
        /// </summary>
        public HttpWebRequest HttpWebRequest { get; set; }

        /// <summary>
        /// Gets or sets the callback function to call when there is a successful response
        /// </summary>
        public AsyncCallback Callback { get; set; }

        /// <summary>
        /// Gets or sets the callback function to call when there is an exception
        /// </summary>
        public AsyncCallback ExceptionCallback { get; set; }

        /// <summary>
        /// Timer Tracking the Time Out Of The Http Request
        /// </summary>
        public HttpWebRequestTimer HttpWebRequestTimer { get; set; }
    }
}
