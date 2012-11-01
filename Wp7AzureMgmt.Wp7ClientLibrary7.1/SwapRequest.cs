// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : UpdateDeploymentStatusRequest.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="SwapRequest.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Threading;
    using Wp7AzureMgmt.Models;

    public class SwapRequest : Request
    {
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Name of the Hosted Service That Contains the Deployment
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the login for the user to the web service
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password for the user to the web service
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or set the currrent production deployment name
        /// </summary>
        public Guid Production { get; set; }

        /// <summary>
        /// Gets or set the current source deployment name
        /// </summary>
        public Guid SourceDeployment { get; set; }

        public string AppVersion { get; set; }

        public String User { get; set; }

        //public AzureFunctionCheckResponse AzureFunctionCheckResponse { get; set; }
        public AzureFunctionCheckResponse ResultResponse { get; set; }
    }
}
