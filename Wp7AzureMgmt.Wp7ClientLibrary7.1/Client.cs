///////////////////////////////////////////////////////////////////////////////
// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : Client.cs
// Purpose      : 
// ============================================================================
// Trial is only based back on GetDeployments since every other request
// hits that one
//
// </todo>
// <copyright file="Client.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Threading;
    using Wp7AzureMgmt.Models;
    using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Client to Call Web Services
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Http Request TimeOut In Milliseonds
        /// </summary>
        /// DFB: Wayne & I had an interesting discussion where maybe the concept of time on the phone
        /// doesn't matter because the user can just start button away from the app
        /// if they don't like how long things are taking - this allows azure to take its time, 
        /// which it does and the user will eventually get the data
        private readonly TimeSpan httpRequestTimeOut = new TimeSpan(0, 0, 180);

        //private readonly string Protocol;

        /// <summary>
        /// Initializes a new instance of the Client class.
        /// </summary>
        /// <param name="webServiceDomain">Domain name of the Web Service</param>
        public Client(string webServiceDomain)
        {
            this.WebServiceDomain = webServiceDomain;

//#if DEBUG
//            Protocol = "http"; 
//#else
//            Protocol = "https";
//#endif
        }

        /// <summary>
        /// Gets or sets the domain name of the web service
        /// </summary>
        private string WebServiceDomain { get; set; }

        #region Configuration

        /// <summary>
        /// Gets the Deployment Information For the Registered Subscriptions From the Web Service
        /// </summary>
        /// <param name="callback">Callback after successful response.</param>
        /// <param name="exceptionCallback">Callback if there is an exception.</param>
        /// <param name="state">State Information To Preserve Across Callbacks</param>
        public void GetConfigurationFromWebService(AsyncCallback callback, AsyncCallback exceptionCallback, ConfigurationRequest request)
        {

            Uri uri = new Uri(String.Format("{0}/Configuration/Index/", this.WebServiceDomain)); 

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<ConfigurationRequest> requestState = new InternalRequestState<ConfigurationRequest>()
            {
                ExternalRequestState = request,
                HttpWebRequest = httpWebRequest,
                Callback = callback,
                ExceptionCallback = exceptionCallback
            };

            // Make the Actual HTTP Request on A Background Worker Thread
            httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseConfigurationCallback), requestState);
        }

        /// <summary>
        /// Callback from Request For Deployments
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseConfigurationCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<ConfigurationRequest> requestState = (InternalRequestState<ConfigurationRequest>)asynchronousResult.AsyncState;

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    ((ConfigurationRequest)requestState.ExternalRequestState).Configuration = ParseConfiguration(httpWebReponse);

                    requestState.Callback(new WebServiceResponse<ConfigurationRequest>(requestState.ExternalRequestState));
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
                    requestState.ExceptionCallback(new WebServiceResponse<Exception>(exception));
                }
            }
        }

        private static Configuration ParseConfiguration(HttpWebResponse httpWebResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ConfigurationResponse));

            var response = serializer.ReadObject(httpWebResponse.GetResponseStream()) as ConfigurationResponse;

            // WWB: Check The Response For Errors
            if (response.Status != ConfigurationResponseStatus.Success)
            {
                var exception = new WebServiceException<ConfigurationResponseStatus>()
                {
                    Status = response.Status
                };

                throw exception;
            }

            return response.Configuration;
        }

        #endregion

        #region Update Deployment

        /// <summary>
        /// Start/Stop/Delete (Swap handled in different method)
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="exceptionCallback"></param>
        /// <param name="state"></param>
        public void UpdateDeploymentStatus(AsyncCallback callback, AsyncCallback exceptionCallback, UpdateDeploymentStatusRequest state)
        {
            // OLD CALL
            // Uri uri = new Uri(String.Format("{0}/Deployment/{1}/{2}?login={3}&password={4}&HostedServiceName={5}&DeploymentName={6}&trial=0&AppVersion={7}", this.WebServiceDomain, state.Status, state.SubscriptionId, state.Login.Trim(), state.Password, state.ServiceName, state.DeploymentName, state.AppVersion));

            // NEW CALL
            Uri uri = new Uri(String.Format("{0}/Deployment/{1}Mango/{2}?login={3}&password={4}&HostedServiceName={5}&DeploymentName={6}&trial=0&AppVersion={7}", this.WebServiceDomain, state.Status, state.SubscriptionId, state.Login.Trim(), state.Password, state.ServiceName, state.DeploymentName, state.AppVersion));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<UpdateDeploymentStatusRequest> requestState = new InternalRequestState<UpdateDeploymentStatusRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseUpdateDeploymentCallback), requestState);
        }

        /// <summary>
        /// Callback from Request For Upating Status
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseUpdateDeploymentCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<UpdateDeploymentStatusRequest> requestState = (InternalRequestState<UpdateDeploymentStatusRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    // Parse response object

                    //requestState.Callback(new WebServiceResponse<UpdateDeploymentStatusRequest>(requestState.ExternalRequestState));

//
                    //requestState.Callback(new WebServiceResponse<DeploymentReinstanceRequest>(requestState.ExternalRequestState));
                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        AzureFunctionCheckResponse response = ParseResponseObjectsOpStatusResponse(httpWebReponse);
                        ((UpdateDeploymentStatusRequest)requestState.ExternalRequestState).ResultResponse = response;
                        //if (response.FunctionCheckResult != null)
                        //{
                        //    ((UpdateDeploymentStatusRequest)requestState.ExternalRequestState).Result = response.FunctionCheckResult;
                        //}

                        requestState.Callback(new WebServiceResponse<UpdateDeploymentStatusRequest>(requestState.ExternalRequestState));
                    }
                    else
                    {
                        AzureFunctionCheckResponse ErrorResponse = new AzureFunctionCheckResponse()
                        {
                            WindowsAzureStatus = AzureFunctionCheckResponseStatus.IllegalResponse
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
                    ExceptionReponse<UpdateDeploymentStatusRequest> response = new ExceptionReponse<UpdateDeploymentStatusRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<UpdateDeploymentStatusRequest>>(response));
                }
            }
        }
        /// <summary>
        /// Callback from Request For Upating Status
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        //private static void HttpWebResponseUpdateDeploymentCallback2(IAsyncResult asynchronousResult)
        //{
        //    // State of request is asynchronous.
        //    InternalRequestState<UpdateDeploymentStatusRequest> requestState = (InternalRequestState<UpdateDeploymentStatusRequest>)asynchronousResult.AsyncState;

        //    // WWB: Stop The Time Out Timer -- Request Has Completed
        //    requestState.HttpWebRequestTimer.SafeStop();

        //    try
        //    {
        //        HttpWebRequest httpWebRequest = requestState.HttpWebRequest;

        //        // Response
        //        using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
        //        {
        //            if (httpWebReponse.ContentType.Contains("application/json"))
        //            {
        //                AzureFunctionCheckResponse response = ParseResponseObjectsOpStatusResponse(httpWebReponse);

        //                if (response != null)
        //                {
        //                    (requestState.ExternalRequestState).AzureFunctionCheckResponse = response;
        //                }

        //                requestState.Callback(new WebServiceResponse<UpdateDeploymentStatusRequest>(requestState.ExternalRequestState));
        //            }
        //            else
        //            {
        //                AzureFunctionCheckResponse ErrorResponse = new AzureFunctionCheckResponse()
        //                {
        //                    WindowsAzureStatus = AzureFunctionCheckResponseStatus.IllegalResponse
        //                };
        //            }

        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        // WWB: All Exception Are Handled By The Exception Callback
        //        if (requestState.ExceptionCallback == null)
        //        {
        //            throw;
        //        }
        //        else
        //        {
        //            // WWB: Package Up A Exception Response Object
        //            ExceptionReponse<UpdateDeploymentStatusRequest> response = new ExceptionReponse<UpdateDeploymentStatusRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

        //            // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
        //            requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<UpdateDeploymentStatusRequest>>(response));
        //        }
        //    }
        //}

        #endregion

        #region Swap Deployment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="exceptionCallback"></param>
        /// <param name="state"></param>
        public void SwapDeployments(AsyncCallback callback, AsyncCallback exceptionCallback, SwapRequest state)
        {
            //OLD CALL
            //Uri uri = new Uri(String.Format("{0}/Deployment/Swap/{1}?login={2}&password={3}&HostedServiceName={4}&Production={5}&SourceDeployment={6}&trial=0&AppVersion={7}", this.WebServiceDomain, state.SubscriptionId, state.Login.Trim(), state.Password, state.ServiceName, state.Production, state.SourceDeployment, state.AppVersion));

            //NEW CALL
            Uri uri = new Uri(String.Format("{0}/Deployment/SwapMango/{1}?login={2}&password={3}&HostedServiceName={4}&Production={5}&SourceDeployment={6}&trial=0&AppVersion={7}", this.WebServiceDomain, state.SubscriptionId, state.Login.Trim(), state.Password, state.ServiceName, state.Production, state.SourceDeployment, state.AppVersion));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<SwapRequest> requestState = new InternalRequestState<SwapRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseSwapDeploymentsCallback), requestState);
        }

        /// <summary>
        /// Callback from Request For Upating Status
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseSwapDeploymentsCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<SwapRequest> requestState = (InternalRequestState<SwapRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    // WWB: At Some Poiny the Response Might Have Something That We Want To Retrieve
                    // Do That Here.

                    //requestState.Callback(new WebServiceResponse<SwapRequest>(requestState.ExternalRequestState));

                    //
                    //requestState.Callback(new WebServiceResponse<DeploymentReinstanceRequest>(requestState.ExternalRequestState));
                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        AzureFunctionCheckResponse response = ParseResponseObjectsOpStatusResponse(httpWebReponse);
                        ((SwapRequest)requestState.ExternalRequestState).ResultResponse = response;

                        requestState.Callback(new WebServiceResponse<SwapRequest>(requestState.ExternalRequestState));
                    }
                    else
                    {
                        AzureFunctionCheckResponse ErrorResponse = new AzureFunctionCheckResponse()
                        {
                            WindowsAzureStatus = AzureFunctionCheckResponseStatus.IllegalResponse
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
                    ExceptionReponse<SwapRequest> response = new ExceptionReponse<SwapRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<SwapRequest>>(response));
                }
            }
        }

        #endregion

        #region Deployments

        /// <summary>
        /// Gets the Deployment Information For the Registered Subscriptions From the Web Service
        /// </summary>
        /// <param name="callback">Callback after successful response.</param>
        /// <param name="exceptionCallback">Callback if there is an exception.</param>
        /// <param name="state">State Information To Preserve Across Callbacks</param>
        public void GetDeploymentsFromWebService(AsyncCallback callback, AsyncCallback exceptionCallback, DeploymentRequest state)
        {
 
            // DFB: Trial flag is only on Get Deployments - always pass trial
            String stringUri = String.Format("{0}/Subscription/Deployments/?login={1}&password={2}&trial={3}&AppVersion={4}", this.WebServiceDomain, state.Login.Trim(), state.Password, state.TrialRemaining,state.AppVersion);

            Uri uri = new Uri(stringUri);


            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<DeploymentRequest> requestState = new InternalRequestState<DeploymentRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseDeploymentsCallback), requestState);
        }

        /// <summary>
        /// WWB: Ticks When Request Times Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void httpRequestTimer_Tick(object sender, EventArgs e)
        {
            HttpWebRequestTimer httpWebRequestTimer = (HttpWebRequestTimer)sender;
            httpWebRequestTimer.HttpWebRequest.Abort();
        }

        /// <summary>
        /// Callback from Request For Deployments
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseDeploymentsCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<DeploymentRequest> requestState = (InternalRequestState<DeploymentRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {

                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        DeploymentsResponse response = ParseResponseObjectsDeploymentsResponse(httpWebReponse);

                        ((DeploymentRequest)requestState.ExternalRequestState).Deployments = response.Deployments;

                        if (response.Trial != null)
                        {
                            ((DeploymentRequest)requestState.ExternalRequestState).TrialRemaining = response.Trial.TrialRemaining;
                            //((DeploymentRequest)requestState.ExternalRequestState).TrialRemaining = response.TrialResponse;
                        }

                        requestState.Callback(new WebServiceResponse<DeploymentRequest>(requestState.ExternalRequestState));
                    }
                    else
                    {
                        throw new Exception("IllegalResponse");
                        //DeploymentsResponse ErrorResponse = new DeploymentsResponse()
                        //{
                        //    Status = DeploymentResponseStatus.IllegalResponse,
                        //    Deployments = null,
                        //    //TrialResponse = 1
                        //    Trial = null
                        //};

                        //requestState.Callback(new WebServiceResponse<DeploymentRequest>(requestState.ExternalRequestState));
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
                    ExceptionReponse<DeploymentRequest> response = new ExceptionReponse<DeploymentRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<DeploymentRequest>>(response));
                }
            }
        }

        /// <summary>
        /// Parse the Response For Deployments And Return Them
        /// </summary>
        /// <param name="httpWebResponse">The Response From the Web Service</param>
        /// <returns>List of Deployments</returns>
        private static DeploymentsResponse ParseResponseObjectsDeploymentsResponse(HttpWebResponse httpWebResponse)
        {
            //try
            //{
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //if (httpWebResponse.ContentType==)

                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DeploymentsResponse));

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
                        var response = serializer.ReadObject(stream) as DeploymentsResponse;

                        // WWB: Check The Response For Errors
                        if (response.Status != DeploymentResponseStatus.Success)
                        {
                            var exception = new WebServiceException<DeploymentResponseStatus>()
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
                    DeploymentsResponse ErrorResponse = new DeploymentsResponse()
                    {
                              Status = DeploymentResponseStatus.IllegalResponse,
                              Deployments=null,
                              Trial=null
                              //TrialResponse=1
                    };

                    return ErrorResponse;
                }
        }
        private static AzureFunctionCheckResponse ParseResponseObjectsOpStatusResponse(HttpWebResponse httpWebResponse)
        {
            //try
            //{
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                //if (httpWebResponse.ContentType==)

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AzureFunctionCheckResponse));

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
                    var response = serializer.ReadObject(stream) as AzureFunctionCheckResponse;

                    // WWB: Check The Response For Errors
                    if (response.WindowsAzureStatus != AzureFunctionCheckResponseStatus.Success)
                    {
                        var exception = new WebServiceException<AzureFunctionCheckResponseStatus>()
                        {
                            Status = response.WindowsAzureStatus
                        };

                        throw exception;
                    }

                    return response;
                }
            }
            else
            {
                AzureFunctionCheckResponse ErrorResponse = new AzureFunctionCheckResponse()
                {
                    WindowsAzureStatus = AzureFunctionCheckResponseStatus.IllegalResponse,
                    FunctionCheckResult = null,
                    RequestId = String.Empty,
                    FunctionName = String.Empty
                };

                return ErrorResponse;
            }
        }

        private static DeploymentsResponse HandleResponseErrorObjectMoved()
        {
            
            DeploymentsResponse ErrorResponse = new DeploymentsResponse()
            {
                  Status = DeploymentResponseStatus.IllegalResponse,
                  Deployments=null,
                  Trial=null
                  //TrialResponse=0
            };

            return ErrorResponse;
        }

        #endregion

        #region Deployment Instance Reboot Reimage

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="exceptionCallback"></param>
        /// <param name="state"></param>
        public void Reinstance(AsyncCallback callback, AsyncCallback exceptionCallback, DeploymentReinstanceRequest state)
        {
            String stringURL = this.WebServiceDomain;

            if (state.InstanceAction == "reboot")
            {
                // stringURL += "/Deployment/ReInstance/"; // OLD CALL
                stringURL += "/Deployment/RebootMango/";  // NEW CALL
            }
            else
            { // reimage
                // stringURL += "/Deployment/ReInstance/"; // OLD CALL
                stringURL += "/Deployment/ReimageMango/";  // NEW CALL
            }

            stringURL += HttpUtility.UrlEncode(state.SubscriptionId.ToString());
            stringURL += "?login=" + HttpUtility.UrlEncode(state.Login.Trim());
            stringURL += "&password=" + HttpUtility.UrlEncode(state.Password);
            stringURL += "&HostedServiceName=" + HttpUtility.UrlEncode(state.ServiceName);
            stringURL += "&DeploymentName=" + HttpUtility.UrlEncode(state.Deployment.ToString());
            stringURL += "&AppVersion=" + HttpUtility.UrlEncode(state.AppVersion);
            stringURL += "&RoleInstanceName=" + HttpUtility.UrlEncode(state.Instance);
            stringURL += "&InstanceAction=" + HttpUtility.UrlEncode(state.InstanceAction);

            Uri uri = new Uri(stringURL);

            //Uri uri = new Uri(String.Format("{0}/Deployment/Reinstace/{1}?login={2}&password={3}&HostedServiceName={4}&DeploymentName={6}&AppVersion={7}&RoleInstanceName{8}&InstanceAction={9}", this.WebServiceDomain, state.SubscriptionId, state.Login.Trim(), state.Password, state.ServiceName, state.Deployment, state.AppVersion, state.Instance, state.InstanceAction));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AllowAutoRedirect = false;

            // WWB: Setup Timer for tracking Http request time out
            HttpWebRequestTimer httpRequestTimer = new HttpWebRequestTimer(httpWebRequest, httpRequestTimeOut);

            // WWB: If The Timer Ticks, This Is The Method That Handles the Timeout
            httpRequestTimer.Elapsed += new ElapsedEventHandler(httpRequestTimer_Tick);

            // Create a Packet Of Information To Surive The Aysnc Call
            InternalRequestState<DeploymentReinstanceRequest> requestState = new InternalRequestState<DeploymentReinstanceRequest>()
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
            IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(HttpWebResponseReinstanceCallback), requestState);
        }

        /// <summary>
        /// Callback from Request For Upating Status
        /// </summary>
        /// <param name="asynchronousResult">Aync Response From Request</param>
        private static void HttpWebResponseReinstanceCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            InternalRequestState<DeploymentReinstanceRequest> requestState = (InternalRequestState<DeploymentReinstanceRequest>)asynchronousResult.AsyncState;

            // WWB: Stop The Time Out Timer -- Request Has Completed
            requestState.HttpWebRequestTimer.SafeStop();

            try
            {
                HttpWebRequest httpWebRequest = requestState.HttpWebRequest;
                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult))
                {
                    //requestState.Callback(new WebServiceResponse<DeploymentReinstanceRequest>(requestState.ExternalRequestState));
                    if (httpWebReponse.ContentType.Contains("application/json"))
                    {
                        AzureFunctionCheckResponse response = ParseResponseObjectsOpStatusResponse(httpWebReponse);
                        ((DeploymentReinstanceRequest)requestState.ExternalRequestState).ResultResponse = response;

                        requestState.Callback(new WebServiceResponse<DeploymentReinstanceRequest>(requestState.ExternalRequestState));
                    }
                    else
                    {
                        AzureFunctionCheckResponse ErrorResponse = new AzureFunctionCheckResponse()
                        {
                            WindowsAzureStatus = AzureFunctionCheckResponseStatus.IllegalResponse
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
                    ExceptionReponse<DeploymentReinstanceRequest> response = new ExceptionReponse<DeploymentReinstanceRequest>() { Exception = exception, Request = requestState.ExternalRequestState };

                    // Call The Exception Call Back With the Exception Response To Report Exceptions To The Caller
                    requestState.ExceptionCallback(new WebServiceResponse<ExceptionReponse<DeploymentReinstanceRequest>>(response));
                }
            }
        }

        #endregion


    }
}