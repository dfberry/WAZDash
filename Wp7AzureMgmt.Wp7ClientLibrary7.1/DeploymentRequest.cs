///////////////////////////////////////////////////////////////////////////////
// Project      : Wp7AzureMgmt.Wp7ClientLibrary
// FileName     : DeploymentRequest.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="DeploymentRequest.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using Wp7AzureMgmt.Models;

    /// <summary>
    /// Input and Output When Requesting Information About Deployments 
    /// From the Web Service
    /// </summary>
    public class DeploymentRequest : Request
    {
        /// <summary>
        /// Gets or sets the deployments returned from request
        /// </summary>
        public Wp7AzureMgmt.Models.Deployment[] Deployments { get; set; }

        /// <summary>
        /// Gets or sets the login for the user to the web service
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password for the user to the web service
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Request [1 == trial, -1 == not trial]
        /// Response [0=expired trial, >0 nonexpired trial, -1 = not trial]
        /// </summary>
        public int TrialRemaining { get; set; }

        public string AppVersion { get; set; }
    }
}
