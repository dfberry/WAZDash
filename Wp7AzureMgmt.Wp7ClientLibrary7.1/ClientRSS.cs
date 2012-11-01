using System;
using System.IO;
using System.Net;
//using System.Windows;
using Wp7AzureMgmt.Wp7ClientLibrary.Models;
using System.Runtime.Serialization.Json;
using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
using System.Xml.Serialization;
using System.Text;

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    public class RSSClient
    {
        private readonly TimeSpan httpRequestTimeOut = new TimeSpan(0, 0, 180);

        /// <summary>
        /// Gets the RSS Information For the Registered Subscriptions From the Web Service
        /// </summary>
        /// <param name="callback">Callback after successful response.</param>
        /// <param name="exceptionCallback">Callback if there is an exception.</param>
        /// <param name="state">State Information To Preserve Across Callbacks</param>
        public void GetRSSFromWebService(AsyncCallback callback, AsyncCallback exceptionCallback, RSSRequest state)
        {
            if (state == null)
                throw new ArgumentNullException("state is null");

            if (String.IsNullOrEmpty(state.ServiceCode))
                throw new ArgumentNullException("state.servicecode is null");

            Uri uri = new Uri(String.Format("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode={0}", state.ServiceCode));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<RSSRequest> requestState = new InternalRequestState<RSSRequest>()
            {
                ExternalRequestState = state, // has service code and location
                HttpWebRequest = httpWebRequest, // has uri
                Callback = callback, 
                ExceptionCallback = exceptionCallback,
                HttpWebRequestTimer = httpRequestTimer
            };

            // WWB: Start The Time Out Timer
            httpRequestTimer.Start();

            // Make the Actual HTTP Request on A Background Worker Thread
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseRSSCallback), requestState);
        }

        /// <summary>
        /// WWB: Ticks When Request Times Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void httpRequestTimer_Tick(object sender, ElapsedEventArgs e)
        {
            HttpWebRequestTimer httpWebRequestTimer = (HttpWebRequestTimer)sender;
            httpWebRequestTimer.HttpWebRequest.Abort();
        }

        /// <summary>
        /// Callback from Request For RSS
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseRSSCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<RSSRequest> requestState = (InternalRequestState<RSSRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    RSSResponse response = ParseResponseObjectsToXML(httpWebReponse);
                    ((RSSRequest)requestState.ExternalRequestState).Reader = response.Reader;

                    requestState.Callback(new WebServiceResponse<RSSRequest>(requestState.ExternalRequestState));
                }
            }
            catch (Exception exception)
            {
                // WWB: All Exception Are Handled By The Exception Callback
                if (requestState.ExceptionCallback == null)
                {
                    throw;
                }
                else
                {
                    // WWB: Package Up A Exception Response Object
                    ExceptionReponse<RSSRequest> response = new ExceptionReponse<RSSRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<RSSRequest>>(response));
                }
            }
        }
        /// <summary>
        /// Parse the Response For RSS And Return Them
        /// </summary>
        /// <param name="httpWebResponse">The Response From the Web Service</param>
        /// <returns>List of RSS</returns>
        public static RSSResponse ParseResponseObjectsToXML(HttpWebResponse httpWebResponse)
        {
            RSSResponse response = new RSSResponse(); 

            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    string xml = streamReader.ReadToEnd();

                    //return Deserialize<T>(xml, Encoding.GetEncoding(httpWebResponse.CharacterSet));
                    response.Reader =  Deserialize(xml, null);
                    response.Status = RSSResponseStatus.Success;

                    return response;
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (Stream responseStream = webException.Response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                //Trace.TraceError(reader.ReadToEnd());
                                response.Status = RSSResponseStatus.Exception;
                            }
                        }
                    }
                }

                throw;
            }
       }
        public static RSS Deserialize(string xml, Encoding encoding)
        {
            if (String.IsNullOrEmpty(xml))
            {
                throw new IndexOutOfRangeException("Response From Web Service is empty");
            }           
            
            RSS rss = Activator.CreateInstance<RSS>();

            XmlSerializer serializer = new XmlSerializer(rss.GetType());

            using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml)))
            {
                return (RSS)serializer.Deserialize(memoryStream);
            }
        }

        ///// <summary>
        ///// Parse the Response For RSS And Return Them
        ///// </summary>
        ///// <param name="httpWebResponse">The Response From the Web Service</param>
        ///// <returns>List of RSS</returns>
        //private static RSSResponse ParseResponseObjectsToJson(HttpWebResponse httpWebResponse)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RSSResponse));

        //    if (httpWebResponse.ContentLength > int.MaxValue)
        //    {
        //        throw new IndexOutOfRangeException(String.Format("Response From Web Service Exceeds {0}", int.MaxValue));
        //    }

        //    // WWB: Read The Data From The Response Stream
        //    int length = (int)httpWebResponse.ContentLength;
        //    byte[] data = new byte[length];
        //    using (Stream responseStream = httpWebResponse.GetResponseStream())
        //    {
        //        responseStream.Read(data, 0, length);
        //    }

        //    // WWB: In A Memory Stream Convert the Json
        //    using (MemoryStream stream = new MemoryStream(data))
        //    {
        //        var response = serializer.ReadObject(stream) as RSSResponse;

        //        // WWB: Check The Response For Errors
        //        if (response.Status != RSSResponseStatus.Success)
        //        {
        //            var exception = new WebServiceException<RSSResponseStatus>()
        //            {
        //                Status = response.Status
        //            };

        //            throw exception;
        //        }

        //        return response;
        //    }
        //}

        //private List<string> AzureServiceDashboardURLs = new List<string>();

        //public AzureServiceDashboard()
        //{
        //    // AppFabricAccessControl
        //    AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSACSWE");

        //    ////AppFabric Access Control 2.0
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2EA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2NCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2NE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2SCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2SEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=ACS2WE");

        //    ////AppFabric Caching
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=AFCWE");

        //    ////AppFabric Portal
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSPortWW");

        //    ////AppFabric Service Bus
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBSNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBSNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBSSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=NSSBWE");

        //    ////Database Manager
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DBMWE");

        //    ////SQL Azure Database
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SAEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SANCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SANE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SASCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SASEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SAWE");

        //    ////SQL Azure Reporting
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=SARSSCUS");

        //    ////Windows Azure CDN
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACDNWW");

        //    ////Windows Azure Compute
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WACWE");

        //    ////Windows Azure Connect 
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WAConnectWW");

        //    ////Windows Azure Marketplace - DataMarket
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=DMSCUS");

        //    ////Windows Azure Service Management
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASvcMgmtWW");

        //    ////Windows Azure Storage
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASNCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASNE");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASSCUS");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASSEA");
        //    //AzureServiceDashboardURLs.Add("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=WASWE");

        //}
        //public List<RSS> GetDashboard()
        //{

        //    RSSFeedFactory AzureServiceDashboardFactory = new RSSFeedFactory();
        //    List<RSS> AzureServiceDashboardFeeds = new List<RSS>();

        //    AzureServiceDashboardFeeds = AzureServiceDashboardFactory.GetFeeds(AzureServiceDashboardURLs);

        //    return AzureServiceDashboardFeeds;
        //}
    }
}
