

namespace Wp7AzureMgmt.Models
{

    using System;
    using System.Collections.Generic;
    
    public enum AzureFunctionCheckResponseStatus
    {
        Success = 0,
        InvalidUser,
        Exception,
        // User Has Registered Windows Azure Subscription, however
        // hasn't added the public key certificate for the subscription
        CertificatesNotApplied,

        /// <summary>
        /// 
        /// </summary>
        NoSubscriptions,
        // RESTRequest (QueryString) either didn't have all the parts
        // or the parts weren't correct
        MalformedRequest,
        IllegalRequest, //operation wasn't allowed based on state at time of request
        IllegalResponse,
        ServerNotFound
    }

    public class AzureFunctionCheckResponse
    {
        /// <summary>
        /// List of Deployments In Registered Subscriptions
        /// </summary>
        public AzureFunctionCheckResponseStatus WindowsAzureStatus { get; set; }

        /// <summary>
        /// RequestId returned from WA Get Operation Status
        /// </summary>
        public String RequestId { get; set; }

        /// <summary>
        /// HTTP Status returned from WA Get Operation Status
        /// </summary>
        public FunctionCheckResult FunctionCheckResult { get; set; }

        /// <summary>
        /// Name of Operation (Noun/Verb) 
        /// 
        /// </summary>
        public String FunctionName { get; set; }

        public DeploymentsResponse  DeploymentsResponse{ get; set; }

    }

    public class FunctionCheckResultError
    {
        public String ErrorCode { get; set; }

        public String Message { get; set; }
    }

    public class FunctionCheckResult
    {
        public String RequestId { get; set; }

        public String Status { get; set; }

        public String HttpStatusCode { get; set; }

        public FunctionCheckResultError[] Errors { get; set; }
    }
}