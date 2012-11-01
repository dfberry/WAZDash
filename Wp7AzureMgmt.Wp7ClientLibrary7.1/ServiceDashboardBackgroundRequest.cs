namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using Wp7AzureMgmt.Models;

    public class ServiceDashboardBackgroundRequest : Request
    {
        public DateTime LastFetchDateTime { get; set; }

        public ServiceDashboardBackground ServiceDashboardBackground { get; set; }
    }
}
