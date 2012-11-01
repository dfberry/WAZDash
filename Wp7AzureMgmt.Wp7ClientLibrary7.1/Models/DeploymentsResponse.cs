
namespace Wp7AzureMgmt.Models
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public enum DeploymentResponseStatus
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

    public class DeploymentsResponse
    {
        /// <summary>
        /// List of Deployments In Registered Subscriptions
        /// </summary>
        public Deployment[] Deployments { get; set; }

        public DeploymentResponseStatus Status { get; set; }

        public Wp7AzureMgmt.Models.Trial Trial { get; set; }

        //public int TrialResponse { get; set; }

        public DeploymentsResponse()
        {
            //'ctor' - test to figure out serialization issue
            //int x = 0;
        }
    }
}