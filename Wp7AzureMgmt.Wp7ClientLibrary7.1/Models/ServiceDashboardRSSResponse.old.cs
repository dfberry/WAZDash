namespace Wp7AzureMgmt.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wp7AzureMgmt.WebRole.Models;
    using Wp7AzureMgmt.Security.Model;

    public enum ServiceDashboardResponseStatus
    {
        Success = 0,
        Exception,
        MalformedRequest,
        IllegalRequest, //operation wasn't allowed based on state at time of request
        IllegalResponse,
        ServerNotFound
    }

    public class ServiceDashboardResponse
    {
        /// <summary>
        /// List of Deployments In Registered Subscriptions
        /// </summary>
        public ServiceDashboardFeed[] Dashboard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServiceDashboardResponseStatus Status { get; set; }

    }
}