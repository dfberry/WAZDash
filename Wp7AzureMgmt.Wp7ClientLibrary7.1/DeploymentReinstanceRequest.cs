   
namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Threading;
    using Wp7AzureMgmt.Models;

    public class DeploymentReinstanceRequest : Request
    {
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Name of the Hosted Service That Contains the Deployment
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or set the current source deployment name
        /// </summary>
        public Guid Deployment { get; set; }

        /// <summary>
        /// Gets or set the current source deployment name
        /// </summary>
        public String Instance { get; set; }

   

        /// <summary>
        /// Gets or sets the login for the user to the web service
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password for the user to the web service
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the appversion from the phone 
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Gets or sets the phone anon id and manufacturer
        /// </summary>
        public String User { get; set; }

        //public FunctionCheckResult Result { get; set; }
        public AzureFunctionCheckResponse ResultResponse { get; set; }
        /// <summary>
        /// Gets or set the requested action: reboot or reimage
        /// </summary>
        public String InstanceAction { get; set; } 
    
    }
}
