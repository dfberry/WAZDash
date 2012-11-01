namespace Wp7AzureMgmt.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public enum ServiceDashboardResponseStatus
    {
        Success = 0,
        Exception,
        MalformedRequest,
        IllegalRequest, //operation wasn't allowed based on state at time of request
        IllegalResponse,
        ServerNotFound
    }
    public class ServiceDashboardPhoneBackgroundResponse
    {
        public ServiceDashboardResponseStatus Status { get; set; }

        public ServiceDashboardBackground ServiceDashboardBackground { get; set; }
    }
    public class ServiceDashboardBackground
    {
        public int CountOfItems { get; set; }

        public DateTime ResponseDateTime { get; set; }
    }
    public class ServiceDashboardResponse
    {
        /// <summary>
        /// List of Deployments In Registered Subscriptions
        /// </summary>
        public ServiceDashboard Dashboard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServiceDashboardResponseStatus Status { get; set; }

        public int IssueCount { get; set; }
    }
    public class ServiceDashboard
    {
        /// <summary>
        /// List of Deployments In Registered Subscriptions
        /// </summary>
        public ServiceDashboardFeed[] DashboardFeeds { get; set; }

        // # of days back from today to get issue items
        // -1 means all
        public int DayRange { get; set; }

        public bool FetchAll { get; set; }

        public string UserPhoneId { get; set; }

    }
    public class ServiceDashboardFeed
    {

        public ServiceDashboardChannel[] FeedResults { get; set; }

        public int IssueCount { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string LocationCode { get; set; }

        public string LocationName { get; set; }

    }
    public class ServiceDashboardChannel
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string PubDate { get; set; }

        public string LastBuildDate { get; set; }

        public string Copyright { get; set; }

        public ServiceDashboardChannelImage Image { get; set; }

        public ServiceDashboardChannelItemDetail[] Items { get; set; }
    }
    public class ServiceDashboardChannelItemDetail
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }

        public DateTime PubDateAsDate { get; set; }
    }
    public class ServiceDashboardChannelImage
    {
        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string RSSUrl { get; set; }

    }
}