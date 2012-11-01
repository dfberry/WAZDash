using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Wp7AzureMgmt.Models;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
using System.Runtime.Serialization.Json;

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    public class ClientServiceDashboard
    {
        private readonly TimeSpan httpRequestTimeOut = new TimeSpan(0, 0, 180);

        private string WebServiceDomain { get; set; }

        public ClientServiceDashboard(string webServiceDomain)
        {
            this.WebServiceDomain = webServiceDomain;

//#if DEBUG
//            Protocol = "http"; 
//#else
//            Protocol = "https";
//#endif
        }

        /// <summary>
        /// Gets the RSS Information For the Registered Subscriptions From the Web Service
        /// ServiceDashboard/Feeds/?anonuser=test
        /// </summary>
        /// <param name="callback">Callback after successful response.</param>
        /// <param name="exceptionCallback">Callback if there is an exception.</param>
        /// <param name="state">State Information To Preserve Across Callbacks</param>
        public void GetAll(AsyncCallback callback, AsyncCallback exceptionCallback, ServiceDashboardRequest state)
        {

            Uri uri = new Uri(String.Format(WebServiceDomain + "/ServiceDashboard/GetAll/?anonuser={0}&issueage={1}&fetchall={2}&appversion={3}", HttpUtility.UrlEncode(state.User), state.IssueAge, state.FetchAllIncludingEmpties, state.AppVersion));
            //Uri uri = new Uri(String.Format(WebServiceDomain + "/ServiceDashboard/GetAll/?anonuser=test&issueage=14&fetchall=0"));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<ServiceDashboardRequest> requestState = new InternalRequestState<ServiceDashboardRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseServiceDashboardCallback), requestState);
        }
        /// <summary>
        /// Gets the RSS Information For the Registered Subscriptions From the Web Service
        /// ServiceDashboard/Feeds/?anonuser=test
        /// </summary>
        /// <param name="callback">Callback after successful response.</param>
        /// <param name="exceptionCallback">Callback if there is an exception.</param>
        /// <param name="state">State Information To Preserve Across Callbacks</param>
        public void FetchSince(AsyncCallback callback, AsyncCallback exceptionCallback, ServiceDashboardRequest state)
        {

            Uri uri = new Uri(String.Format(WebServiceDomain + "/ServiceDashboard/FetchSince/?anonuser={0}&issueage={1}&fetchall={2}&appversion={3}&summary={4}", HttpUtility.UrlEncode(state.User), state.IssueAge, state.FetchAllIncludingEmpties, state.AppVersion,state.Summary));
//            Uri uri = new Uri(String.Format(WebServiceDomain + "/ServiceDashboard/GetAll/?anonuser={0}&issueage={1}&fetchall={2}&appversion={3}", HttpUtility.UrlEncode(state.User), state.IssueAge, state.FetchAllIncludingEmpties, state.AppVersion));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<ServiceDashboardRequest> requestState = new InternalRequestState<ServiceDashboardRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseServiceDashboardCallback), requestState);
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
        private static void HttpWebResponseServiceDashboardCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<ServiceDashboardRequest> requestState = (InternalRequestState<ServiceDashboardRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        ServiceDashboardResponse response = ParseResponseFromJson(httpWebReponse);

                        ((ServiceDashboardRequest)requestState.ExternalRequestState).Dashboard = response.Dashboard;

                        requestState.Callback(new WebServiceResponse<ServiceDashboardRequest>(requestState.ExternalRequestState));
                    }
                    else //assume error
                    {
                        ServiceDashboardResponse ErrorResponse = new ServiceDashboardResponse()
                        {
                            Status = ServiceDashboardResponseStatus.IllegalResponse,
                            Dashboard=null
                        };
                    }
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
                    ExceptionReponse<ServiceDashboardRequest> response = new ExceptionReponse<ServiceDashboardRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<ServiceDashboardRequest>>(response));
                }
            }
        }
        /// <summary>
        /// Parse the Response For Deployments And Return Them
        /// </summary>
        /// <param name="httpWebResponse">The Response From the Web Service</param>
        /// <returns>List of Deployments</returns>
        private static ServiceDashboardResponse ParseResponseFromJson(HttpWebResponse httpWebResponse)
        {
            //try
            //{
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                //if (httpWebResponse.ContentType==)

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceDashboardResponse));

                if (httpWebResponse.ContentLength > int.MaxValue)
                {
                    throw new IndexOutOfRangeException(String.Format("Response From Web Service Exceeds {0}", int.MaxValue));
                }

                // WWB: Read The Data From The Response Stream
                int length = (int)httpWebResponse.ContentLength;
                byte[] data = new byte[length];
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    responseStream.Read(data, 0, length);
                }

                // WWB: In A Memory Stream Convert the Json
                using (MemoryStream stream = new MemoryStream(data))
                {
                    var response = serializer.ReadObject(stream) as ServiceDashboardResponse;

                    // WWB: Check The Response For Errors
                    if (response.Status != ServiceDashboardResponseStatus.Success)
                    {
                        var exception = new WebServiceException<ServiceDashboardResponseStatus>()
                        {
                            Status = response.Status
                        };

                        throw exception;
                    }

                    return response;
                }
            }
            else
            {
                ServiceDashboardResponse ErrorResponse = new ServiceDashboardResponse()
                {
                    Status = ServiceDashboardResponseStatus.IllegalResponse,
                    Dashboard = null
                };

                return ErrorResponse;
            }
            //}
            //catch (Exception ex)
            //{
            //    String message = ex.Message;
            //    //if (ex is SerializationException)
            //    //{
            //    //    return ServiceDashboardResponse ErrorResponse = new ServiceDashboardResponse()
            //    //        {
            //    //              Status = ServiceDashboardResponse.IllegalResponse,
                                    //Dashboard = null,
                                    //ServiceCode = String.Empty
            //    //        };
            //    //}
            //    throw new Exception();
            //}
        }

    }
}
