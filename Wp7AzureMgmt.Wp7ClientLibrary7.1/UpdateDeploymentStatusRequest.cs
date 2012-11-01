// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : UpdateDeploymentStatusRequest.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="UpdateDeploymentStatusRequest.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using Wp7AzureMgmt.Models;

    public class UpdateDeploymentStatusRequest : Request
    {
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Name of the Hosted Service That Contains the Deployment
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Name of the Deployment
        /// </summary>
        public Guid DeploymentName { get; set; }

        /// <summary>
        /// Gets or sets the login for the user to the web service
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password for the user to the web service
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or set the status to update the deployment to
        /// </summary>
        public DeploymentStatus Status { get; set; }

        public string AppVersion { get; set; }

        // Response status from Windows Azure
        //public FunctionCheckResult Result { get; set; }
        public AzureFunctionCheckResponse ResultResponse { get; set; }
        //public AzureFunctionCheckResponse AzureFunctionCheckResponse { get; set; }
    }
}
