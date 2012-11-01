///////////////////////////////////////////////////////////////////////////////
// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : HttpWebRequestTimer.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="HttpWebRequestTimer.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////


namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Windows.Threading;
    using System.Net;

    /// <summary>
    /// Class To Encapsulate The Http Web Request In a Timer
    /// </summary>
    public class HttpWebRequestTimer : DispatcherTimer
    {
        /// <summary>
        /// Http Web Request Associated With the Timer
        /// </summary>
        public HttpWebRequest HttpWebRequest { get; private set; }

        /// <summary>
        /// Initalize a new instance of the HttpWebRequestTimer class.
        /// </summary>
        /// <param name="httpWebRequest"></param>
        public HttpWebRequestTimer(HttpWebRequest httpWebRequest, TimeSpan interval)
        {
            this.HttpWebRequest = httpWebRequest;
            this.Interval = interval;
        }

        /// <summary>
        /// Stops The Timer On the Dispatcher Thread
        /// </summary>
        public void SafeStop()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    base.Stop();
                });
        }

        public event ElapsedEventHandler Elapsed;
    }
}
