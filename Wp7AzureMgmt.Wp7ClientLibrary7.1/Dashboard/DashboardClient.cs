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
using System.IO;
using System.Runtime.Serialization.Json;
using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
using Wp7AzureMgmt.Wp7ClientLibrary.Utilities;

namespace Wp7AzureMgmt.Wp7ClientLibrary.Dashboard
{
    public class DashboardClient
    {
        private readonly TimeSpan httpRequestTimeOut = new TimeSpan(0, 0, 180);

        /// <summary>
        /// https://wazdash.berryintl.com
        /// or http://locahost:81
        /// </summary>
        private string WebServiceDomain { get; set; }

        /// <summary>
        /// Constructor needs to now beginning part of web request
        /// </summary>
        /// <param name="webServiceDomain"></param>
        public DashboardClient(string webServiceDomain)
        {
            this.WebServiceDomain = webServiceDomain;
        }
        void httpRequestTimer_Tick(object sender, ElapsedEventArgs e)
        {
            HttpWebRequestTimer httpWebRequestTimer = (HttpWebRequestTimer)sender;
            httpWebRequestTimer.HttpWebRequest.Abort();
        }
        public void BeginGetFilledIssues(AsyncCallback callback, AsyncCallback exceptionCallback, DashboardRequest state)
        {
            Uri uri = new Uri(String.Format(WebServiceDomain + "/Issues/Get/?" +
                "anonuser={0}" +
                "&issueage={1}" +
                "&appversion={2}" +
                "&trial={3}" +
                "&phoneid={4}" +
                "&phonemaker={5}" +
                "&sidx={6}" +
                "&sord={7}" +
                "&page={8}" +
                "&rows={9}",
                HttpUtility.UrlEncode(state.UserId),
                HttpUtility.UrlEncode(state.IssueAge.ToString()),
                HttpUtility.UrlEncode(state.AppVersion),
                HttpUtility.UrlEncode(state.TrialRemaining.ToString()),
                HttpUtility.UrlEncode(state.PhoneId),
                HttpUtility.UrlEncode(state.PhoneMaker),
                "IssueDate",    // sidx
                "desc",         // order
                "1",            // page
                "500"           // rows
                ));
            

            // previous method -before conversion
            //Uri uri = new Uri(String.Format(WebServiceDomain + "/ServiceDashboard/GetFilled2/?" + 
            //    "anonuser={0}" + 
            //    "&issueage={1}" + 
            //    "&appversion={2}" + 
            //    "&trial={3}" +
            //    "&phoneid={4}" +
            //    "&phonemaker={5}", 
            //    HttpUtility.UrlEncode(state.UserId), 
            //    HttpUtility.UrlEncode(state.IssueAge.ToString()), 
            //    HttpUtility.UrlEncode(state.AppVersion), 
            //    HttpUtility.UrlEncode(state.TrialRemaining.ToString()),
            //    HttpUtility.UrlEncode(state.PhoneId),
            //    HttpUtility.UrlEncode(state.PhoneMaker)));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<DashboardRequest> requestState = new InternalRequestState<DashboardRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(EndGetFilledIssues), requestState);
        }
        private static void EndGetFilledIssues(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<DashboardRequest> requestState = (InternalRequestState<DashboardRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        DashboardResponse response = JsonResponse.Parse<DashboardResponse>(httpWebReponse);

                        ((DashboardRequest)requestState.ExternalRequestState).DashboardResponse = response;

                        requestState.Callback(new WebServiceResponse<DashboardRequest>(requestState.ExternalRequestState));
                    }
                    else //assume error
                    {
                        DashboardResponse ErrorResponse = new DashboardResponse()
                        {
                            Status = Wp7AzureMgmt.Models.ResponseStatus.IllegalResponse,
                            List = null
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
                    ExceptionReponse<DashboardRequest> response = new ExceptionReponse<DashboardRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<DashboardRequest>>(response));
                }
            }
        }
        

    }
    
}
