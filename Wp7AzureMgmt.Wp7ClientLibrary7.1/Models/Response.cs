///////////////////////////////////////////////////////////////////////////////
// Project      : 
// FileName     : Response.cs
// Purpose      : 
// ============================================================================
// <todo>
//
// </todo>
// <copyright file="Response.cs" company="Berry International, Inc.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////

namespace Wp7AzureMgmt.Models
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public enum ResponseStatus
    {
        Success = 0,
        InvalidUser, 
        Exception,
        MalformedRequest,
        IllegalRequest, //operation wasn't allowed based on state at time of request
        IllegalResponse,
        ServerNotFound,    
        CertificatesNotApplied,
        NoSubscriptions
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response
    {
        /// <summary>
        /// 
        /// </summary>
        //public T Reader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResponseStatus Status { get; set; }
    }
}
