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

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    public class ClientException
    {
        private readonly TimeSpan httpRequestTimeOut = new TimeSpan(0, 0, 180);

        private string WebServiceDomain { get; set; }

        public ClientException(string webServiceDomain)
        {
            this.WebServiceDomain = webServiceDomain;

        }
        void httpRequestTimer_Tick(object sender, ElapsedEventArgs e)
        {
            HttpWebRequestTimer httpWebRequestTimer = (HttpWebRequestTimer)sender;
            httpWebRequestTimer.HttpWebRequest.Abort();
        }
        public void AppExceptionCall(AsyncCallback callback, AsyncCallback exceptionCallback, ExceptionRequest state)
        {
            String stringURL = this.WebServiceDomain;
            stringURL += "/App/Exception/?";  

            stringURL += "login=" + HttpUtility.UrlEncode(state.Login);
            stringURL += "&appversion=" + HttpUtility.UrlEncode(state.AppVersion);
            stringURL += "&appname=" + HttpUtility.UrlEncode(state.AppName);
            stringURL += "&phoneid=" + HttpUtility.UrlEncode(state.PhoneId);
            stringURL += "&exception=" + HttpUtility.UrlEncode(state.Exception);

            Uri uri = new Uri(stringURL);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<ExceptionRequest> requestState = new InternalRequestState<ExceptionRequest>()
            {
                ExternalRequestState = state,
                HttpWebRequest = httpWebRequest,
                Callback = callback,
                ExceptionCallback = exceptionCallback,
                HttpWebRequestTimer = httpRequestTimer
            };

            // WWB: Start The Time Out Timer
            httpRequestTimer.Start();

            // Make the Actual HTTP Request on A Background Worker Thread
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(AppExceptionCallback), requestState);
            //IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, requestState);
        }
       private static void AppExceptionCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<ExceptionRequest> requestState = (InternalRequestState<ExceptionRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            //try
            //{
            //    HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
            //    using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
            //    {
            //        if (httpWebReponse.StatusCode == HttpStatusCode.OK)
            //        {
            //            requestState.Callback(new WebServiceResponse<ExceptionRequest>(requestState.ExternalRequestState));
            //        }
            //        else
            //        {
            //            // do something meaningful here
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    // WWB: All Exception Are Handled By The Exception Callback
            //    if (requestState.ExceptionCallback == null)
            //    {
            //        throw;
            //    }
            //    else
            //    {
            //        ExceptionReponse<ExceptionRequest> response = new ExceptionReponse<ExceptionRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

            //        // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
            //        requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<ExceptionRequest>>(response));
            //    }
            //}
        }

    }
}
