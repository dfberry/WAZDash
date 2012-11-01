///////////////////////////////////////////////////////////////////////////////
// Project      : 
// FileName     : Deployment.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="Deployment.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Models
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;

    /// <summary>
    /// Class for representing deployments
    /// </summary>
    public class Deployment
    {
        /// <summary>
        /// Gets or sets the subscription identifier in which the deployment is running
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Name of the subscription in which the deployment is running
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Name of the hosted service in which the deployment is running
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Unique Identifier of Hosted Service (subnet) 
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// The affinity group with which this hosted service is associated, if any. If the service is associated with an
        /// affinity group, the Location element is not returned.
        /// </summary>
        public string ServiceAffinityGroup { get; set; }

        /// <summary>
        /// The label for the hosted service.
        /// </summary>
        public string ServiceLabel { get; set; }

        /// <summary>
        /// The label for the hosted service location.
        /// </summary>
        public string ServiceLocation { get; set; }

        /// <summary>
        /// The description for the hosted service.
        /// </summary>
        public string SerivceDescription { get; set; }

        /// <summary>
        /// Name of the deployment (REST refers to this as the Label)
        /// </summary>
        public string DeploymentLabel { get; set; }

        /// <summary>
        /// Identifier of the Deployment (REST refers to this as the Name)
        /// </summary>
        public Guid DeploymentName { get; set; }

        /// <summary>
        /// The deployment environment in which this deployment is running, either Staging or Production. 
        /// </summary>
        public string DeploymentSlot { get; set; }

        /// <summary>
        /// Url For The Deployment
        /// </summary>
        public string DeploymentUrl { get; set; }

        /// <summary>
        /// The unique identifier for this deployment.
        /// </summary>
        public string DeploymentPrivateID { get; set; }

        /// <summary>
        /// The status of the deployment. Possible values are:
        ///     Running, Suspended, RunningTransitioning, SuspendedTransitioning, Starting, Suspending, Deploying, Deleting             
        /// </summary>
        public string DeploymentStatus { get; set; }

        /// <summary>
        /// The Instances Under the Deployment
        /// </summary>
        public Models.Instance[] Instances { get; set; }

        /// <summary>
        /// Numbers of domains total that are upgraded one at a time
        /// </summary>
        public int UpgradeDomainCount { get; set; }

        /// <summary>
        /// SDK Version
        /// </summary>
        public string SdkVersion { get; set; }

        public int[] DeploymentEndpointPorts { get; set; }

        /// <summary>
        /// True if the deployment is swapable, false if it is not.
        /// </summary>
        public bool CanDeploymentsSwap { get; set; }

        /// <summary>
        /// Deployment Name of the Deployment in the Other Slot
        /// </summary>
        public Guid Peer { get; set; }
    }
}