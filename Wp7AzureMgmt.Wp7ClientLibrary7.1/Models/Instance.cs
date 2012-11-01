///////////////////////////////////////////////////////////////////////////////
// Project      : 
// FileName     : Instance.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="Instance.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Models
{
    using System;

    /// <summary>
    /// Represents a Windows Azure Instance
    /// </summary>
    public class Instance
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the specific role instance, if an instance of the role is running.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the current status of this instance. Possible values are: 
        ///     Ready, Busy, Initializing, Stopping, Stopped, Unresponsive 
        /// </summary>
        public string InstanceStatus { get; set; }
    }
}