///////////////////////////////////////////////////////////////////////////////
// Project      : Wp7AzureMgmt.WebRole
// FileName     : Configuration.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="Configuration.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Models
{
    using System;

    /// <summary>
    /// Configuration Information Sent To The Windows 7 Phone
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the regular expression used to evaluate a password.
        /// </summary>
        public string PasswordStrengthRegularExpression { get; set; }

        /// <summary>
        /// Gets or sets the minimum length required for a password.
        /// </summary>
        public int MinRequiredPasswordLength { get; set; }
    }
}