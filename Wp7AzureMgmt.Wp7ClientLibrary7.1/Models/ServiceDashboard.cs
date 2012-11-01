namespace Wp7AzureMgmt.Wp7ClientLibrary.Models
{
    using System;
    using System.Net;
    
    public class ServiceDashboardFeed
    {
            
        public RSSFeedChannel[] FeedResults { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string LocationCode { get; set; }

        public string LocationName { get; set; }

    }
    public class RSSFeedChannel
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string PubDate { get; set; }

        public string LastBuildDate { get; set; }

        public string Copyright { get; set; }

        public string Image { get; set; }

        public RSSFeedItemDetail[] Item { get; set; }
    }
    public class RSSFeedItemDetail
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }
    }
    public class ServiceDashboardRequest : Request
    {
        /// <summary>
        /// Gets or sets the deployments returned from request
        /// </summary>
        public ServiceDashboardFeed[] Dashboard { get; set; }

        public string ServiceCode { get; set; }

        // anything specific to that phone, probably make my own Guid
        public string AnonUser { get; set; }
    }
   
}
